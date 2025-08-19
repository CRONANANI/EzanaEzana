using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [StringLength(500)]
        public string? Bio { get; set; }
        
        public string? ProfilePictureUrl { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        [StringLength(100)]
        public string? Occupation { get; set; }
        
        [StringLength(100)]
        public string? Company { get; set; }
        
        public bool IsPublicProfile { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
        
        public int LoginCount { get; set; } = 0;
        
        public string? TimeZone { get; set; }
        
        public string? PreferredLanguage { get; set; } = "en-US";
        
        public bool EmailNotificationsEnabled { get; set; } = true;
        
        public bool PushNotificationsEnabled { get; set; } = true;
        
        public bool SMSNotificationsEnabled { get; set; } = false;
        
        // Navigation Properties
        public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
        public virtual ICollection<CommunityThread> AuthoredThreads { get; set; } = new List<CommunityThread>();
        public virtual ICollection<ThreadComment> AuthoredComments { get; set; } = new List<ThreadComment>();
        public virtual ICollection<ThreadLike> ThreadLikes { get; set; } = new List<ThreadLike>();
        public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public virtual ICollection<ThreadView> ThreadViews { get; set; } = new List<ThreadView>();
        public virtual ICollection<CommunityMembership> CommunityMemberships { get; set; } = new List<CommunityMembership>();
        public virtual ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public virtual ICollection<Watchlist> Watchlists { get; set; } = new List<Watchlist>();
        public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
        public virtual ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
        public virtual ICollection<UserStatistics> UserStatistics { get; set; } = new List<UserStatistics>();
        public virtual ICollection<Friendship> RequestedFriendships { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> ReceivedFriendships { get; set; } = new List<Friendship>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public virtual ICollection<InvestmentPreference> InvestmentPreferences { get; set; } = new List<InvestmentPreference>();
        public virtual ICollection<GRPVModel> GRPVAnalyses { get; set; } = new List<GRPVModel>();
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName ?? "Unknown User";
        public bool HasProfilePicture => !string.IsNullOrEmpty(ProfilePictureUrl);
        public int Age => DateOfBirth.HasValue ? DateTime.Today.Year - DateOfBirth.Value.Year : 0;
        public bool IsActive => LastLoginAt > DateTime.UtcNow.AddDays(-30);
    }
} 