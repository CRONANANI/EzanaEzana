using Ezana.Models;
using Ezana.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ezana.Services
{
    public interface ISocialService
    {
        // User search
        Task<List<UserSearchResultViewModel>> SearchUsers(string searchTerm, string currentUserId);
        Task<ProfileViewModel> GetUserProfile(string username, string currentUserId);
        
        // Friendship management
        Task<FriendsViewModel> GetUserFriends(string userId);
        Task<bool> SendFriendRequest(string requesterId, string addresseeId);
        Task<bool> AcceptFriendRequest(int requestId, string userId);
        Task<bool> RejectFriendRequest(int requestId, string userId);
        Task<bool> CancelFriendRequest(int requestId, string userId);
        Task<bool> RemoveFriend(string userId, string friendId);
        Task<ViewModels.FriendshipStatus?> GetFriendshipStatus(string userId, string otherUserId);
        
        // Messaging
        Task<MessagesViewModel> GetUserConversations(string userId);
        Task<ConversationViewModel> GetConversation(string userId, string otherUserId);
        Task<MessageViewModel> SendMessage(string senderId, string recipientId, string content);
        Task<bool> MarkMessageAsRead(int messageId, string userId);
        Task<int> GetUnreadMessageCount(string userId);
    }
} 