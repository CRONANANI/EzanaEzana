using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Social ViewModels
    public class UserSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public List<UserProfileViewModel> Users { get; set; } = new List<UserProfileViewModel>();
        public int TotalResults { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool HasMoreResults { get; set; }
    }

    public class UserSearchResultViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool IsFriend { get; set; }
        public bool HasPendingFriendRequest { get; set; }
        public bool HasSentFriendRequest { get; set; }
    }

    public class UserProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Location { get; set; }
        public string? Occupation { get; set; }
        public string? Company { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsPublicProfile { get; set; }
        public string? FriendshipStatus { get; set; }
        public bool CanSendFriendRequest { get; set; }
        public bool CanSendMessage { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);
        public bool IsActive => LastLoginAt > DateTime.UtcNow.AddDays(-30);
    }

    public enum FriendshipStatus
    {
        NotFriends,
        Friends,
        RequestSent,
        RequestReceived
    }

    public class FriendsViewModel
    {
        public List<FriendViewModel> Friends { get; set; } = new List<FriendViewModel>();
        public List<FriendRequestViewModel> PendingRequests { get; set; } = new List<FriendRequestViewModel>();
        public List<FriendRequestViewModel> SentRequests { get; set; } = new List<FriendRequestViewModel>();
        public int TotalFriends { get; set; }
        public int PendingRequestsCount { get; set; }
        public int SentRequestsCount { get; set; }
    }

    public class FriendViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Location { get; set; }
        public string? Occupation { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime FriendshipDate { get; set; }
        public bool IsOnline { get; set; }
        public string? LastActivity { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);
    }

    public class FriendRequestViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Message { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);
        public bool IsPending => Status.Equals("Pending", StringComparison.OrdinalIgnoreCase);
    }

    public class MessagesViewModel
    {
        public List<ConversationViewModel> Conversations { get; set; } = new List<ConversationViewModel>();
        public ConversationViewModel? CurrentConversation { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
        public int TotalConversations { get; set; }
        public int UnreadCount { get; set; }
    }

    public class ConversationViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string OtherUserId { get; set; } = string.Empty;
        public string OtherUserName { get; set; } = string.Empty;
        public string? OtherUserFirstName { get; set; }
        public string? OtherUserLastName { get; set; }
        public string? OtherUserProfilePictureUrl { get; set; }
        public string? LastMessage { get; set; }
        public DateTime LastMessageAt { get; set; }
        public bool IsUnread { get; set; }
        public int UnreadCount { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Computed Properties
        public string OtherUserFullName => $"{OtherUserFirstName} {OtherUserLastName}".Trim();
        public string OtherUserDisplayName => !string.IsNullOrEmpty(OtherUserFirstName) ? OtherUserFirstName : OtherUserName;
        public bool HasOtherUserProfilePicture => !string.IsNullOrEmpty(OtherUserProfilePictureUrl);
        public string LastMessagePreview => !string.IsNullOrEmpty(LastMessage) && LastMessage.Length > 50 ? LastMessage.Substring(0, 50) + "..." : LastMessage ?? "";
    }

    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string SenderUserName { get; set; } = string.Empty;
        public string? SenderFirstName { get; set; }
        public string? SenderLastName { get; set; }
        public string? SenderProfilePictureUrl { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsFromCurrentUser { get; set; }
        
        // Computed Properties
        public string SenderFullName => $"{SenderFirstName} {SenderLastName}".Trim();
        public string SenderDisplayName => !string.IsNullOrEmpty(SenderFirstName) ? SenderFirstName : SenderUserName;
        public bool HasSenderProfilePicture => !string.IsNullOrEmpty(SenderProfilePictureUrl);
        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.UtcNow - SentAt;
                if (timeSpan.TotalMinutes < 1) return "Just now";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m ago";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h ago";
                if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays}d ago";
                return SentAt.ToString("MMM dd");
            }
        }
    }

    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Location { get; set; }
        public string? Occupation { get; set; }
        public string? Company { get; set; }
        public bool IsPublicProfile { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public int LoginCount { get; set; }
        public string? TimeZone { get; set; }
        public string? PreferredLanguage { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool PushNotificationsEnabled { get; set; }
        public bool SMSNotificationsEnabled { get; set; }
        public bool IsCurrentUser { get; set; }
        public bool CanEdit { get; set; }
        public bool CanSendFriendRequest { get; set; }
        public bool CanSendMessage { get; set; }
        public string? FriendshipStatus { get; set; }
        public List<AchievementViewModel> RecentAchievements { get; set; } = new List<AchievementViewModel>();
        public List<UserActivityViewModel> RecentActivity { get; set; } = new List<UserActivityViewModel>();
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);
        public int Age => DateOfBirth.HasValue ? DateTime.Today.Year - DateOfBirth.Value.Year : 0;
        public bool IsActive => LastLoginAt > DateTime.UtcNow.AddDays(-30);
        public string MemberSince => CreatedAt.ToString("MMMM yyyy");
    }

    public class UserActivityViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
        
        // Computed Properties
        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.UtcNow - CreatedAt;
                if (timeSpan.TotalMinutes < 1) return "Just now";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m ago";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h ago";
                if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays}d ago";
                return CreatedAt.ToString("MMM dd");
            }
        }
    }

    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Message content is required")]
        [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters")]
        public string Content { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Recipient is required")]
        public string RecipientId { get; set; } = string.Empty;
    }

    public class SendFriendRequestViewModel
    {
        [Required(ErrorMessage = "Recipient is required")]
        public string RecipientId { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters")]
        public string? Message { get; set; }
    }

    public class UpdateProfileViewModel
    {
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters")]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [StringLength(100, ErrorMessage = "Occupation cannot be longer than 100 characters")]
        [Display(Name = "Occupation")]
        public string? Occupation { get; set; }

        [StringLength(100, ErrorMessage = "Company cannot be longer than 100 characters")]
        [Display(Name = "Company")]
        public string? Company { get; set; }

        [Display(Name = "Public Profile")]
        public bool IsPublicProfile { get; set; }

        [StringLength(100, ErrorMessage = "Time zone cannot be longer than 100 characters")]
        [Display(Name = "Time Zone")]
        public string? TimeZone { get; set; }

        [StringLength(10, ErrorMessage = "Preferred language cannot be longer than 10 characters")]
        [Display(Name = "Preferred Language")]
        public string? PreferredLanguage { get; set; }

        [Display(Name = "Email Notifications")]
        public bool EmailNotificationsEnabled { get; set; }

        [Display(Name = "Push Notifications")]
        public bool PushNotificationsEnabled { get; set; }

        [Display(Name = "SMS Notifications")]
        public bool SMSNotificationsEnabled { get; set; }
    }
} 