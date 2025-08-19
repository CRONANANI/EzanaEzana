using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Community Thread ViewModels
    public class CreateThreadViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [StringLength(500)]
        public string Tags { get; set; } = string.Empty; // Comma-separated tags
    }

    public class UpdateThreadViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [StringLength(500)]
        public string Tags { get; set; } = string.Empty;
    }

    // Thread Comment ViewModels
    public class AddCommentViewModel
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public int? ParentCommentId { get; set; } // For nested replies
    }

    public class UpdateCommentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
    }

    // Thread Statistics ViewModels
    public class ThreadStatisticsViewModel
    {
        public int ThreadId { get; set; }
        public string ThreadTitle { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int ReplyCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class UserCommunityStatsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int TotalThreads { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public int TotalViews { get; set; }
        public DateTime MemberSince { get; set; }
        public DateTime LastActivity { get; set; }
        public string Reputation { get; set; } = string.Empty;
        public List<string> TopCategories { get; set; } = new List<string>();
    }

    // Community Search and Display ViewModels
    public class ThreadsViewModel
    {
        public List<ThreadStatisticsViewModel> Threads { get; set; } = new List<ThreadStatisticsViewModel>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalThreads { get; set; }
        public int TotalPages { get; set; }
        public string? Category { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class ThreadDetailViewModel
    {
        public ThreadStatisticsViewModel Thread { get; set; } = new ThreadStatisticsViewModel();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public bool IsLiked { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanModerate { get; set; }
    }

    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
    }

    public class SearchResultsViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public List<ThreadStatisticsViewModel> Threads { get; set; } = new List<ThreadStatisticsViewModel>();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<UserCommunityStatsViewModel> Users { get; set; } = new List<UserCommunityStatsViewModel>();
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string? Category { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class UserThreadsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<ThreadStatisticsViewModel> Threads { get; set; } = new List<ThreadStatisticsViewModel>();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public int TotalThreads { get; set; }
        public int TotalComments { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
