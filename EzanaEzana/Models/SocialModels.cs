using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models
{
    // Social System
    public class Friendship
    {
        public int Id { get; set; }
        
        [Required]
        public string RequesterId { get; set; } = string.Empty;
        
        [ForeignKey("RequesterId")]
        public virtual ApplicationUser Requester { get; set; } = null!;
        
        [Required]
        public string AddresseeId { get; set; } = string.Empty;
        
        [ForeignKey("AddresseeId")]
        public virtual ApplicationUser Addressee { get; set; } = null!;
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty; // Pending, Accepted, Rejected, Blocked
        
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
        
        [StringLength(500)]
        public string? Message { get; set; }
        
        // Composite unique constraint
        [Index(nameof(RequesterId), nameof(AddresseeId), IsUnique = true)]
        public class FriendshipIndex { }
    }

    public class Message
    {
        public int Id { get; set; }
        
        [Required]
        public string SenderId { get; set; } = string.Empty;
        
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; } = null!;
        
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
        
        [ForeignKey("ReceiverId")]
        public virtual ApplicationUser Receiver { get; set; } = null!;
        
        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;
        
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        
        public int? ReplyToMessageId { get; set; }
        
        [ForeignKey("ReplyToMessageId")]
        public virtual Message? ReplyToMessage { get; set; }
        
        // Navigation properties
        public virtual ICollection<Message> Replies { get; set; } = new List<Message>();
    }

    // Investment Preference System
    public class InvestmentPreference
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [StringLength(50)]
        public string RiskTolerance { get; set; } = string.Empty; // Conservative, Moderate, Aggressive
        
        [StringLength(50)]
        public string InvestmentHorizon { get; set; } = string.Empty; // Short-term, Medium-term, Long-term
        
        public decimal? TargetAnnualReturn { get; set; }
        public decimal? MaxDrawdown { get; set; }
        
        public bool PreferDividends { get; set; }
        public bool PreferGrowth { get; set; }
        public bool PreferValue { get; set; }
        
        [StringLength(500)]
        public string? ExcludedSectors { get; set; } // Comma-separated sectors to avoid
        
        [StringLength(500)]
        public string? PreferredSectors { get; set; } // Comma-separated preferred sectors
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
