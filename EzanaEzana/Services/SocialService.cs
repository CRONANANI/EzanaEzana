using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzanaEzana.Services
{
    public class SocialService : ISocialService
    {
        private readonly ApplicationDbContext _context;

        public SocialService(ApplicationDbContext context)
        {
            _context = context;
        }

        // User search
        public async Task<List<UserSearchResultViewModel>> SearchUsers(string searchTerm, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<UserSearchResultViewModel>();

            var query = _context.Users
                .Where(u => u.PublicUsername != null && u.IsPublicProfile)
                .Where(u => u.Id != currentUserId)
                .Where(u => (u.PublicUsername != null && u.PublicUsername.Contains(searchTerm)) || 
                           (u.FirstName != null && u.FirstName.Contains(searchTerm)) || 
                           (u.LastName != null && u.LastName.Contains(searchTerm)));

            var users = await query.ToListAsync();
            var results = new List<UserSearchResultViewModel>();

            foreach (var user in users)
            {
                if (user.PublicUsername == null) continue;  // Skip users without public username
                
                var status = await GetFriendshipStatus(currentUserId, user.Id);
                
                results.Add(new UserSearchResultViewModel
                {
                    UserId = user.Id,
                    Username = user.PublicUsername,  // We know it's not null due to the check above
                    DisplayName = GetDisplayName(user),
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "",  // Default to empty string if null
                    IsFriend = status == ViewModels.FriendshipStatus.Friends,
                    HasPendingFriendRequest = status == ViewModels.FriendshipStatus.RequestReceived,
                    HasSentFriendRequest = status == ViewModels.FriendshipStatus.RequestSent
                });
            }

            return results;
        }

        public async Task<ProfileViewModel> GetUserProfile(string username, string currentUserId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PublicUsername == username);

            if (user == null || user.PublicUsername == null)
                throw new ArgumentException("User not found");

            var isCurrentUser = user.Id == currentUserId;
            var status = isCurrentUser ? null : await GetFriendshipStatus(currentUserId, user.Id);
            var friendshipId = 0;

            if (status == ViewModels.FriendshipStatus.RequestReceived)
            {
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        (f.RequesterId == currentUserId && f.AddresseeId == user.Id) ||
                        (f.RequesterId == user.Id && f.AddresseeId == currentUserId));
                
                if (friendship != null)
                    friendshipId = friendship.Id;
            }

            return new ProfileViewModel
            {
                UserId = user.Id,
                Username = user.PublicUsername,  // We know it's not null due to the check above
                DisplayName = GetDisplayName(user),
                ProfilePictureUrl = user.ProfilePictureUrl ?? "",  // Default to empty string if null
                Bio = user.Bio ?? "",  // Default to empty string if null
                JoinedDate = user.CreatedAt,
                FriendshipStatus = status,
                FriendshipId = friendshipId,
                IsCurrentUser = isCurrentUser
            };
        }

        // Friendship management
        public async Task<FriendsViewModel> GetUserFriends(string userId)
        {
            var viewModel = new FriendsViewModel();

            // Get accepted friendships
            var acceptedFriendships = await _context.Friendships
                .Where(f => (f.RequesterId == userId || f.AddresseeId == userId) && f.Status == Models.FriendshipStatus.Accepted)
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .ToListAsync();

            var friends = await GetFriendsFromFriendships(acceptedFriendships, userId);
            viewModel.Friends.AddRange(friends);

            // Get pending friend requests (received)
            var pendingRequests = await _context.Friendships
                .Where(f => f.AddresseeId == userId && f.Status == Models.FriendshipStatus.Pending)
                .Include(f => f.Requester)
                .ToListAsync();

            foreach (var request in pendingRequests)
            {
                if (request.Requester?.PublicUsername == null) continue;  // Skip if requester data is invalid
                
                viewModel.PendingRequests.Add(new FriendRequestViewModel
                {
                    RequestId = request.Id,
                    UserId = request.Requester.Id,
                    Username = request.Requester.PublicUsername,
                    DisplayName = GetDisplayName(request.Requester),
                    ProfilePictureUrl = request.Requester.ProfilePictureUrl ?? "",
                    RequestDate = request.CreatedAt
                });
            }

            // Get sent friend requests
            var sentRequests = await _context.Friendships
                .Where(f => f.RequesterId == userId && f.Status == Models.FriendshipStatus.Pending)
                .Include(f => f.Addressee)
                .ToListAsync();

            foreach (var request in sentRequests)
            {
                if (request.Addressee?.PublicUsername == null) continue;  // Skip if addressee data is invalid
                
                viewModel.SentRequests.Add(new FriendRequestViewModel
                {
                    RequestId = request.Id,
                    UserId = request.Addressee.Id,
                    Username = request.Addressee.PublicUsername,
                    DisplayName = GetDisplayName(request.Addressee),
                    ProfilePictureUrl = request.Addressee.ProfilePictureUrl ?? "",
                    RequestDate = request.CreatedAt
                });
            }

            return viewModel;
        }

        public async Task<bool> SendFriendRequest(string requesterId, string addresseeId)
        {
            // Check if there's already a friendship between these users
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f => 
                    (f.RequesterId == requesterId && f.AddresseeId == addresseeId) ||
                    (f.RequesterId == addresseeId && f.AddresseeId == requesterId));

            if (existingFriendship != null)
            {
                // If it's already accepted, do nothing
                if (existingFriendship.Status == Models.FriendshipStatus.Accepted)
                    return true;

                // If it was rejected or blocked, update it to pending
                if (existingFriendship.Status == Models.FriendshipStatus.Rejected)
                {
                    existingFriendship.Status = Models.FriendshipStatus.Pending;
                    existingFriendship.RequesterId = requesterId;
                    existingFriendship.AddresseeId = addresseeId;
                    existingFriendship.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }

                // If it's blocked, don't allow a new request
                if (existingFriendship.Status == Models.FriendshipStatus.Blocked)
                    return false;

                // If it's pending, do nothing
                return true;
            }

            // Create a new friendship request
            var friendship = new Friendship
            {
                RequesterId = requesterId,
                AddresseeId = addresseeId,
                Status = Models.FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptFriendRequest(int requestId, string userId)
        {
            var request = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == requestId && f.AddresseeId == userId && f.Status == Models.FriendshipStatus.Pending);

            if (request == null)
                return false;

            request.Status = Models.FriendshipStatus.Accepted;
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectFriendRequest(int requestId, string userId)
        {
            var request = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == requestId && f.AddresseeId == userId && f.Status == Models.FriendshipStatus.Pending);

            if (request == null)
                return false;

            request.Status = Models.FriendshipStatus.Rejected;
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelFriendRequest(int requestId, string userId)
        {
            var request = await _context.Friendships
                .FirstOrDefaultAsync(f => f.Id == requestId && f.RequesterId == userId && f.Status == Models.FriendshipStatus.Pending);

            if (request == null)
                return false;

            _context.Friendships.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFriend(string userId, string friendId)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => 
                    ((f.RequesterId == userId && f.AddresseeId == friendId) ||
                     (f.RequesterId == friendId && f.AddresseeId == userId)) &&
                    f.Status == Models.FriendshipStatus.Accepted);

            if (friendship == null)
                return false;

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ViewModels.FriendshipStatus?> GetFriendshipStatus(string userId, string otherUserId)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => 
                    (f.RequesterId == userId && f.AddresseeId == otherUserId) ||
                    (f.RequesterId == otherUserId && f.AddresseeId == userId));

            if (friendship == null)
                return ViewModels.FriendshipStatus.NotFriends;

            switch (friendship.Status)
            {
                case Models.FriendshipStatus.Accepted:
                    return ViewModels.FriendshipStatus.Friends;
                case Models.FriendshipStatus.Pending:
                    return friendship.RequesterId == userId 
                        ? ViewModels.FriendshipStatus.RequestSent 
                        : ViewModels.FriendshipStatus.RequestReceived;
                case Models.FriendshipStatus.Rejected:
                case Models.FriendshipStatus.Blocked:
                default:
                    return ViewModels.FriendshipStatus.NotFriends;
            }
        }

        // Messaging
        public async Task<MessagesViewModel> GetUserConversations(string userId)
        {
            var viewModel = new MessagesViewModel();

            // Get all conversations (users with whom the current user has exchanged messages)
            var sentMessages = await _context.Messages
                .Where(m => m.SenderId == userId)
                .Select(m => m.RecipientId)
                .Distinct()
                .ToListAsync();

            var receivedMessages = await _context.Messages
                .Where(m => m.RecipientId == userId)
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();

            var conversationUserIds = sentMessages.Union(receivedMessages).Distinct().ToList();

            foreach (var otherUserId in conversationUserIds)
            {
                try
                {
                    var conversation = await GetConversation(userId, otherUserId);
                    if (conversation == null) continue;

                    var summaryViewModel = new ConversationSummaryViewModel
                    {
                        UserId = conversation.UserId,
                        Username = conversation.Username ?? "Unknown User",
                        DisplayName = conversation.DisplayName ?? "Unknown User",
                        ProfilePictureUrl = conversation.ProfilePictureUrl ?? "",
                        LastMessagePreview = conversation.LastMessagePreview ?? "No message",
                        LastMessageDate = conversation.LastMessageDate,
                        HasUnreadMessages = conversation.HasUnreadMessages,
                        UnreadCount = conversation.Messages?.Count(m => !m.IsRead && !m.IsFromCurrentUser) ?? 0
                    };
                    viewModel.Conversations.Add(summaryViewModel);
                }
                catch (ArgumentException)
                {
                    // Skip conversations with invalid users
                    continue;
                }
            }

            // Sort conversations by last message date
            viewModel.Conversations = viewModel.Conversations
                .OrderByDescending(c => c.LastMessageDate)
                .ToList();

            // Get unread count
            viewModel.UnreadCount = await GetUnreadMessageCount(userId);

            return viewModel;
        }

        public async Task<ConversationViewModel> GetConversation(string userId, string otherUserId)
        {
            var otherUser = await _context.Users.FindAsync(otherUserId);
            if (otherUser == null || otherUser.PublicUsername == null)
                throw new ArgumentException("User not found");

            // Get messages between the two users
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .Where(m => 
                    (m.SenderId == userId && m.RecipientId == otherUserId) ||
                    (m.SenderId == otherUserId && m.RecipientId == userId))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var messageViewModels = messages
                .Where(m => m.Sender != null && m.Recipient != null)  // Ensure both sender and recipient exist
                .Select(m => new MessageViewModel
                {
                    MessageId = m.Id,
                    SenderId = m.SenderId,
                    SenderUsername = m.Sender.PublicUsername ?? m.Sender.UserName?.Split('@')?[0] ?? "Unknown User",
                    SenderDisplayName = GetDisplayName(m.Sender),
                    SenderProfilePictureUrl = m.Sender.ProfilePictureUrl ?? "",
                    Content = m.Content ?? "",
                    SentAt = m.CreatedAt,
                    IsRead = m.IsRead,
                    IsFromCurrentUser = m.SenderId == userId
                }).ToList();

            var lastMessage = messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault();
            var hasUnread = messages.Any(m => m.RecipientId == userId && !m.IsRead);

            return new ConversationViewModel
            {
                UserId = otherUserId,
                Username = otherUser.PublicUsername,
                DisplayName = GetDisplayName(otherUser),
                ProfilePictureUrl = otherUser.ProfilePictureUrl ?? "",
                LastMessageDate = lastMessage?.CreatedAt ?? DateTime.MinValue,
                LastMessagePreview = lastMessage?.Content switch
                {
                    null => "No messages yet",
                    var content when content.Length > 30 => content.Substring(0, 27) + "...",
                    var content => content
                },
                HasUnreadMessages = hasUnread,
                Messages = messageViewModels
            };
        }

        public async Task<MessageViewModel> SendMessage(string senderId, string recipientId, string content)
        {
            var sender = await _context.Users.FindAsync(senderId);
            if (sender == null || sender.PublicUsername == null)
                throw new ArgumentException("Sender not found");

            var recipient = await _context.Users.FindAsync(recipientId);
            if (recipient == null)
                throw new ArgumentException("Recipient not found");

            // Check if they are friends
            var areFriends = await _context.Friendships
                .AnyAsync(f => 
                    ((f.RequesterId == senderId && f.AddresseeId == recipientId) ||
                     (f.RequesterId == recipientId && f.AddresseeId == senderId)) &&
                    f.Status == Models.FriendshipStatus.Accepted);

            if (!areFriends)
                throw new InvalidOperationException("You can only send messages to your friends");

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Content = content ?? "",  // Default to empty string if null
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return new MessageViewModel
            {
                MessageId = message.Id,
                SenderId = senderId,
                SenderUsername = sender.PublicUsername,
                SenderDisplayName = GetDisplayName(sender),
                SenderProfilePictureUrl = sender.ProfilePictureUrl ?? "",
                Content = content ?? "",
                SentAt = message.CreatedAt,
                IsRead = false,
                IsFromCurrentUser = true
            };
        }

        public async Task<bool> MarkMessageAsRead(int messageId, string userId)
        {
            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId && m.RecipientId == userId);

            if (message == null)
                return false;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadMessageCount(string userId)
        {
            var friendIds = await _context.Friendships
                .Where(f => 
                    (f.RequesterId == userId || f.AddresseeId == userId) && 
                    f.Status == Models.FriendshipStatus.Accepted)
                .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();
            
            return await _context.Messages
                .CountAsync(m => m.RecipientId == userId && !m.IsRead);
        }

        // Helper methods
        private string GetDisplayName(ApplicationUser user)
        {
            if (user == null) return "User";  // Guard against null user

            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                return $"{user.FirstName} {user.LastName}";
            
            if (!string.IsNullOrEmpty(user.FirstName))
                return user.FirstName;

            if (!string.IsNullOrEmpty(user.PublicUsername))
                return user.PublicUsername;

            return user.UserName?.Split('@')?[0] ?? "User";
        }

        private async Task<List<FriendViewModel>> GetFriendsFromFriendships(List<Friendship> friendships, string userId)
        {
            var friendIds = friendships
                .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
                .ToList();

            var users = await _context.Users
                .Where(u => friendIds.Contains(u.Id))
                .ToListAsync();

            var unreadMessages = await _context.Messages
                .Where(m => 
                    m.RecipientId == userId && 
                    friendIds.Contains(m.SenderId) && 
                    !m.IsRead)
                .GroupBy(m => m.SenderId)
                .Select(g => new { SenderId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.SenderId, x => x.Count);

            return friendships
                .Select(f => 
                {
                    var friendId = f.RequesterId == userId ? f.AddresseeId : f.RequesterId;
                    var friend = users.FirstOrDefault(u => u.Id == friendId);
                    
                    if (friend == null) return null;
                    
                    return new FriendViewModel
                    {
                        UserId = friend.Id,
                        Username = friend.UserName ?? friend.Id,  // Fallback to Id if UserName is null
                        DisplayName = GetDisplayName(friend),
                        ProfilePictureUrl = friend.ProfilePictureUrl ?? "",
                        FriendsSince = f.Status == Models.FriendshipStatus.Accepted ? f.UpdatedAt ?? f.CreatedAt : f.CreatedAt,
                        HasUnreadMessages = unreadMessages.ContainsKey(friendId) && unreadMessages[friendId] > 0
                    };
                })
                .Where(f => f != null)
                .Select(f => f!)
                .ToList();
        }
    }
} 