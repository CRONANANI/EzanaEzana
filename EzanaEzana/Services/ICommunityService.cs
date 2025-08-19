using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.Services
{
    public interface ICommunityService
    {
        // Thread Management
        Task<IEnumerable<CommunityThread>> GetAllThreadsAsync(int page = 1, int pageSize = 20, string? category = null);
        Task<CommunityThread?> GetThreadByIdAsync(int id);
        Task<CommunityThread> CreateThreadAsync(CreateThreadViewModel model, string authorId);
        Task<bool> UpdateThreadAsync(int id, UpdateThreadViewModel model, string userId);
        Task<bool> DeleteThreadAsync(int id, string userId);
        Task<bool> PinThreadAsync(int id, string userId);
        Task<bool> UnpinThreadAsync(int id, string userId);

        // Comment Management
        Task<IEnumerable<ThreadComment>> GetThreadCommentsAsync(int threadId, int page = 1, int pageSize = 50);
        Task<ThreadComment?> GetCommentByIdAsync(int id);
        Task<ThreadComment> AddCommentAsync(int threadId, AddCommentViewModel model, string authorId);
        Task<bool> UpdateCommentAsync(int id, UpdateCommentViewModel model, string userId);
        Task<bool> DeleteCommentAsync(int id, string userId);

        // User Engagement
        Task<bool> LikeThreadAsync(int threadId, string userId);
        Task<bool> UnlikeThreadAsync(int threadId, string userId);
        Task<bool> LikeCommentAsync(int commentId, string userId);
        Task<bool> UnlikeCommentAsync(int commentId, string userId);
        Task<bool> ViewThreadAsync(int threadId, string userId);

        // Thread Statistics
        Task<ThreadStatisticsViewModel> GetThreadStatisticsAsync(int threadId);
        Task<UserCommunityStatsViewModel> GetUserCommunityStatsAsync(string userId);

        // Search and Discovery
        Task<IEnumerable<CommunityThread>> SearchThreadsAsync(string searchTerm, string? category = null, int page = 1, int pageSize = 20);
        Task<IEnumerable<CommunityThread>> GetThreadsByUserAsync(string userId, int page = 1, int pageSize = 20);
        Task<IEnumerable<CommunityThread>> GetThreadsByCategoryAsync(string category, int page = 1, int pageSize = 20);

        // User Activity Logging
        Task LogUserActivityAsync(string userId, string category, string action, string details);
    }
}
