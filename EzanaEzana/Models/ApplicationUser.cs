using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ezana.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
        public string? PublicUsername { get; set; }
        
        public bool IsPublicProfile { get; set; } = false;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual InvestmentPreference? InvestmentPreference { get; set; }
        
        // Social features
        public virtual ICollection<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
} 