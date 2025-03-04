using Ezana.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ezana.ViewModels
{
    // User Search ViewModels
    public class UserSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<UserSearchResultViewModel> Results { get; set; } = new List<UserSearchResultViewModel>();
    }

    public class UserSearchResultViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsFriend { get; set; }
        public bool HasPendingFriendRequest { get; set; }
        public bool HasSentFriendRequest { get; set; }
    }

    // Profile ViewModels
    public class ProfileViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime JoinedDate { get; set; }
        public bool IsCurrentUser { get; set; }
        public FriendshipStatus? FriendshipStatus { get; set; }
        public int? FriendshipId { get; set; }
        public int FriendsCount { get; set; }
        
        // Investment stats (if public profile)
        public bool ShowInvestmentStats { get; set; }
        public decimal? PortfolioValue { get; set; }
        public decimal? PortfolioGrowth { get; set; }
        public int? InvestmentCount { get; set; }
    }

    public enum FriendshipStatus
    {
        NotFriends,
        Friends,
        RequestSent,
        RequestReceived
    }

    // Friends ViewModels
    public class FriendsViewModel
    {
        public List<FriendViewModel> Friends { get; set; } = new List<FriendViewModel>();
        public List<FriendRequestViewModel> PendingRequests { get; set; } = new List<FriendRequestViewModel>();
        public List<FriendRequestViewModel> SentRequests { get; set; } = new List<FriendRequestViewModel>();
    }

    public class FriendViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime FriendsSince { get; set; }
        public bool HasUnreadMessages { get; set; }
    }

    public class FriendRequestViewModel
    {
        public int RequestId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime RequestDate { get; set; }
    }

    // Messaging ViewModels
    public class MessagesViewModel
    {
        public List<ConversationSummaryViewModel> Conversations { get; set; } = new List<ConversationSummaryViewModel>();
        public ConversationViewModel CurrentConversation { get; set; }
        public SendMessageViewModel NewMessage { get; set; }
        public int UnreadCount { get; set; }
    }

    public class ConversationSummaryViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string LastMessagePreview { get; set; }
        public DateTime LastMessageDate { get; set; }
        public bool HasUnreadMessages { get; set; }
        public int UnreadCount { get; set; }
    }

    public class ConversationViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
        public DateTime LastMessageDate { get; set; }
        public string LastMessagePreview { get; set; }
        public bool HasUnreadMessages { get; set; }
    }

    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderDisplayName { get; set; }
        public string SenderProfilePictureUrl { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsFromCurrentUser { get; set; }
    }

    public class SendMessageViewModel
    {
        public string RecipientId { get; set; }

        [Required(ErrorMessage = "Message content is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Content { get; set; }
    }
} 