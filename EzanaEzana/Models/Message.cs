using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ezana.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SenderId { get; set; } = null!;
        
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; } = null!;
        
        [Required]
        public string RecipientId { get; set; } = null!;
        
        [ForeignKey("RecipientId")]
        public virtual ApplicationUser Recipient { get; set; } = null!;
        
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = null!;
        
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }
} 