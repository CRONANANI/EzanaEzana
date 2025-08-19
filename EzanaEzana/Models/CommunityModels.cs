using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models
{
    // Community Thread System
    public class CommunityThread
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; } = null!;
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual CommunityCategory Category { get; set; } = null!;
        
        public bool IsPinned { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<ThreadComment> Comments { get; set; } = new List<ThreadComment>();
        public virtual ICollection<ThreadLike> Likes { get; set; } = new List<ThreadLike>();
        public virtual ICollection<ThreadView> Views { get; set; } = new List<ThreadView>();
        public virtual ICollection<ThreadTag> ThreadTags { get; set; } = new List<ThreadTag>();
    }

    public class CommunityCategory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<CommunityThread> Threads { get; set; } = new List<CommunityThread>();
    }

    public class ThreadComment
    {
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public int ThreadId { get; set; }
        
        [ForeignKey("ThreadId")]
        public virtual CommunityThread Thread { get; set; } = null!;
        
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; } = null!;
        
        public int? ParentCommentId { get; set; }
        
        [ForeignKey("ParentCommentId")]
        public virtual ThreadComment? ParentComment { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ThreadComment> Replies { get; set; } = new List<ThreadComment>();
        public virtual ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
    }

    public class ThreadLike
    {
        public int Id { get; set; }
        
        public int ThreadId { get; set; }
        
        [ForeignKey("ThreadId")]
        public virtual CommunityThread Thread { get; set; } = null!;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(ThreadId), nameof(UserId), IsUnique = true)]
        public class ThreadLikeIndex { }
    }

    public class CommentLike
    {
        public int Id { get; set; }
        
        public int CommentId { get; set; }
        
        [ForeignKey("CommentId")]
        public virtual ThreadComment Comment { get; set; } = null!;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(CommentId), nameof(UserId), IsUnique = true)]
        public class CommentLikeIndex { }
    }

    public class ThreadView
    {
        public int Id { get; set; }
        
        public int ThreadId { get; set; }
        
        [ForeignKey("ThreadId")]
        public virtual CommunityThread Thread { get; set; } = null!;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    public class ThreadTag
    {
        public int Id { get; set; }
        
        public int ThreadId { get; set; }
        
        [ForeignKey("ThreadId")]
        public virtual CommunityThread Thread { get; set; } = null!;
        
        public int TagId { get; set; }
        
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Tag
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<ThreadTag> ThreadTags { get; set; } = new List<ThreadTag>();
    }

    // Community Membership System
    public class CommunityMembership
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public int CommunityId { get; set; }
        
        [ForeignKey("CommunityId")]
        public virtual Community Community { get; set; } = null!;
        
        public string Role { get; set; } = "Member"; // Member, Moderator, Admin
        public bool IsActive { get; set; } = true;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }
    }

    public class Community
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public string Type { get; set; } = "Public"; // Public, Private, InviteOnly
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<CommunityMembership> Members { get; set; } = new List<CommunityMembership>();
    }
}
