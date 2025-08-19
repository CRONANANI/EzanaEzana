using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models
{
    // User Activity System
    public class UserActivity
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty; // Investment, Community, Learning, etc.
        
        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty; // Buy, Sell, Post, Comment, etc.
        
        [StringLength(1000)]
        public string? Details { get; set; }
        
        public string? RelatedEntityType { get; set; } // Portfolio, Thread, Achievement, etc.
        public int? RelatedEntityId { get; set; }
        
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // User Management System
    public class UserNotification
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Message { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // Info, Success, Warning, Error
        
        public string? RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
        
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);
    }

    public class UserPreference
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Value { get; set; }
        
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // String, Number, Boolean, JSON
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(UserId), nameof(Key), IsUnique = true)]
        public class UserPreferenceIndex { }
    }

    public class UserStatistics
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public int TotalPortfolios { get; set; }
        public int TotalHoldings { get; set; }
        public int TotalTransactions { get; set; }
        public int TotalWatchlists { get; set; }
        public int TotalThreads { get; set; }
        public int TotalComments { get; set; }
        public int TotalAchievements { get; set; }
        public int TotalPoints { get; set; }
        
        public decimal TotalPortfolioValue { get; set; }
        public decimal TotalGainLoss { get; set; }
        public decimal TotalGainLossPercent { get; set; }
        
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
