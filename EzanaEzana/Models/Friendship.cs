using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ezana.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string RequesterId { get; set; } = null!;
        
        [ForeignKey("RequesterId")]
        public virtual ApplicationUser Requester { get; set; } = null!;
        
        [Required]
        public string AddresseeId { get; set; } = null!;
        
        [ForeignKey("AddresseeId")]
        public virtual ApplicationUser Addressee { get; set; } = null!;
        
        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
    
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blocked
    }
} 