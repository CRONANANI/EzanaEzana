using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models
{
    // Achievement System
    public class Achievement
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // Investment, Community, Learning, etc.
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // Milestone, Challenge, Special, etc.
        
        [Required]
        [StringLength(100)]
        public string IconClass { get; set; } = string.Empty; // CSS class for icon
        
        [Required]
        [StringLength(50)]
        public string ColorScheme { get; set; } = string.Empty; // CSS color scheme
        
        [Required]
        [Range(1, 1000)]
        public int Points { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }

    public class UserAchievement
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public int AchievementId { get; set; }
        
        [ForeignKey("AchievementId")]
        public virtual Achievement Achievement { get; set; } = null!;
        
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Composite unique constraint
        [Index(nameof(UserId), nameof(AchievementId), IsUnique = true)]
        public class UserAchievementIndex { }
    }
}
