using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommunityService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommunityService(
            ApplicationDbContext context, 
            ILogger<CommunityService> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CommunityThread>> GetAllThreadsAsync(int page = 1, int pageSize = 20, string? category = null)
        {
            try
            {
                var query = _context.CommunityThreads
                    .Include(ct => ct.Author)
                    .Include(ct => ct.Category)
                    .Where(ct => ct.IsActive);

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(ct => ct.Category.Name == category);
                }

                return await query
                    .OrderByDescending(ct => ct.IsPinned)
                    .ThenByDescending(ct => ct.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all threads");
                return new List<CommunityThread>();
            }
        }

        public async Task<CommunityThread?> GetThreadByIdAsync(int id)
        {
            try
            {
                return await _context.CommunityThreads
                    .Include(ct => ct.Author)
                    .Include(ct => ct.Category)
                    .Include(ct => ct.Tags)
                    .FirstOrDefaultAsync(ct => ct.Id == id && ct.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting thread by ID {ThreadId}", id);
                return null;
            }
        }

        public async Task<CommunityThread> CreateThreadAsync(CreateThreadViewModel model, string authorId)
        {
            try
            {
                var thread = new CommunityThread
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = authorId,
                    CategoryId = model.CategoryId,
                    IsPinned = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Add tags if provided
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagNames = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim().ToLower())
                        .Distinct()
                        .ToList();

                    foreach (var tagName in tagNames)
                    {
                        var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tagName, CreatedAt = DateTime.UtcNow };
                            _context.Tags.Add(existingTag);
                        }
                        thread.Tags.Add(existingTag);
                    }
                }

                _context.CommunityThreads.Add(thread);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(authorId, "Community", "Created thread", thread.Id.ToString());

                _logger.LogInformation("Thread {ThreadTitle} created by user {AuthorId}", 
                    model.Title, authorId);

                return thread;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating thread for user {AuthorId}", authorId);
                throw;
            }
        }

        public async Task<bool> UpdateThreadAsync(UpdateThreadViewModel model, string userId)
        {
            try
            {
                var thread = await _context.CommunityThreads
                    .Include(ct => ct.Tags)
                    .FirstOrDefaultAsync(ct => ct.Id == model.Id && ct.AuthorId == userId);

                if (thread == null)
                {
                    return false;
                }

                thread.Title = model.Title;
                thread.Content = model.Content;
                thread.CategoryId = model.CategoryId;
                thread.UpdatedAt = DateTime.UtcNow;

                // Update tags
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagNames = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim().ToLower())
                        .Distinct()
                        .ToList();

                    // Clear existing tags
                    thread.Tags.Clear();

                    foreach (var tagName in tagNames)
                    {
                        var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tagName, CreatedAt = DateTime.UtcNow };
                            _context.Tags.Add(existingTag);
                        }
                        thread.Tags.Add(existingTag);
                    }
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Updated thread", thread.Id.ToString());

                _logger.LogInformation("Thread {ThreadId} updated by user {UserId}", 
                    model.Id, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating thread {ThreadId} by user {UserId}", 
                    model.Id, userId);
                return false;
            }
        }

        public async Task<bool> DeleteThreadAsync(int threadId, string userId)
        {
            try
            {
                var thread = await _context.CommunityThreads
                    .FirstOrDefaultAsync(ct => ct.Id == threadId && ct.AuthorId == userId);

                if (thread == null)
                {
                    return false;
                }

                thread.IsActive = false;
                thread.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Deleted thread", threadId.ToString());

                _logger.LogInformation("Thread {ThreadId} deleted by user {UserId}", 
                    threadId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting thread {ThreadId} by user {UserId}", 
                    threadId, userId);
                return false;
            }
        }

        public async Task<IEnumerable<ThreadComment>> GetThreadCommentsAsync(int threadId, int page = 1, int pageSize = 50)
        {
            try
            {
                return await _context.ThreadComments
                    .Include(tc => tc.Author)
                    .Include(tc => tc.ParentComment)
                    .Where(tc => tc.ThreadId == threadId && tc.IsActive)
                    .OrderBy(tc => tc.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments for thread {ThreadId}", threadId);
                return new List<ThreadComment>();
            }
        }

        public async Task<ThreadComment> AddCommentAsync(AddCommentViewModel model, string authorId)
        {
            try
            {
                var comment = new ThreadComment
                {
                    ThreadId = model.ThreadId,
                    ParentCommentId = model.ParentCommentId,
                    Content = model.Content,
                    AuthorId = authorId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.ThreadComments.Add(comment);

                // Update thread comment count
                var thread = await _context.CommunityThreads.FindAsync(model.ThreadId);
                if (thread != null)
                {
                    thread.CommentCount++;
                    thread.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(authorId, "Community", "Added comment", comment.Id.ToString());

                _logger.LogInformation("Comment added to thread {ThreadId} by user {AuthorId}", 
                    model.ThreadId, authorId);

                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to thread {ThreadId} by user {AuthorId}", 
                    model.ThreadId, authorId);
                throw;
            }
        }

        public async Task<bool> UpdateCommentAsync(UpdateCommentViewModel model, string userId)
        {
            try
            {
                var comment = await _context.ThreadComments
                    .FirstOrDefaultAsync(tc => tc.Id == model.Id && tc.AuthorId == userId);

                if (comment == null)
                {
                    return false;
                }

                comment.Content = model.Content;
                comment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Updated comment", comment.Id.ToString());

                _logger.LogInformation("Comment {CommentId} updated by user {UserId}", 
                    model.Id, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating comment {CommentId} by user {UserId}", 
                    model.Id, userId);
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId, string userId)
        {
            try
            {
                var comment = await _context.ThreadComments
                    .FirstOrDefaultAsync(tc => tc.Id == commentId && tc.AuthorId == userId);

                if (comment == null)
                {
                    return false;
                }

                comment.IsActive = false;
                comment.UpdatedAt = DateTime.UtcNow;

                // Update thread comment count
                var thread = await _context.CommunityThreads.FindAsync(comment.ThreadId);
                if (thread != null)
                {
                    thread.CommentCount = Math.Max(0, thread.CommentCount - 1);
                    thread.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Deleted comment", comment.Id.ToString());

                _logger.LogInformation("Comment {CommentId} deleted by user {UserId}", 
                    commentId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment {CommentId} by user {UserId}", 
                    commentId, userId);
                return false;
            }
        }

        public async Task<bool> LikeThreadAsync(int threadId, string userId)
        {
            try
            {
                var existingLike = await _context.ThreadLikes
                    .FirstOrDefaultAsync(tl => tl.ThreadId == threadId && tl.UserId == userId);

                if (existingLike != null)
                {
                    return false; // User already liked this thread
                }

                var like = new ThreadLike
                {
                    ThreadId = threadId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ThreadLikes.Add(like);

                // Update thread like count
                var thread = await _context.CommunityThreads.FindAsync(threadId);
                if (thread != null)
                {
                    thread.LikeCount++;
                    thread.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Liked thread", threadId.ToString());

                _logger.LogInformation("Thread {ThreadId} liked by user {UserId}", 
                    threadId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking thread {ThreadId} by user {UserId}", 
                    threadId, userId);
                return false;
            }
        }

        public async Task<bool> UnlikeThreadAsync(int threadId, string userId)
        {
            try
            {
                var like = await _context.ThreadLikes
                    .FirstOrDefaultAsync(tl => tl.ThreadId == threadId && tl.UserId == userId);

                if (like == null)
                {
                    return false; // User hasn't liked this thread
                }

                _context.ThreadLikes.Remove(like);

                // Update thread like count
                var thread = await _context.CommunityThreads.FindAsync(threadId);
                if (thread != null)
                {
                    thread.LikeCount = Math.Max(0, thread.LikeCount - 1);
                    thread.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Unliked thread", threadId.ToString());

                _logger.LogInformation("Thread {ThreadId} unliked by user {UserId}", 
                    threadId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unliking thread {ThreadId} by user {UserId}", 
                    threadId, userId);
                return false;
            }
        }

        public async Task<bool> LikeCommentAsync(int commentId, string userId)
        {
            try
            {
                var existingLike = await _context.CommentLikes
                    .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (existingLike != null)
                {
                    return false; // User already liked this comment
                }

                var like = new CommentLike
                {
                    CommentId = commentId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.CommentLikes.Add(like);

                // Update comment like count
                var comment = await _context.ThreadComments.FindAsync(commentId);
                if (comment != null)
                {
                    comment.LikeCount++;
                    comment.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Liked comment", commentId.ToString());

                _logger.LogInformation("Comment {CommentId} liked by user {UserId}", 
                    commentId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking comment {CommentId} by user {UserId}", 
                    commentId, userId);
                return false;
            }
        }

        public async Task<bool> UnlikeCommentAsync(int commentId, string userId)
        {
            try
            {
                var like = await _context.CommentLikes
                    .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (like == null)
                {
                    return false; // User hasn't liked this comment
                }

                _context.CommentLikes.Remove(like);

                // Update comment like count
                var comment = await _context.ThreadComments.FindAsync(commentId);
                if (comment != null)
                {
                    comment.LikeCount = Math.Max(0, comment.LikeCount - 1);
                    comment.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Community", "Unliked comment", commentId.ToString());

                _logger.LogInformation("Comment {CommentId} unliked by user {UserId}", 
                    commentId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unliking comment {CommentId} by user {UserId}", 
                    commentId, userId);
                return false;
            }
        }

        public async Task<bool> ViewThreadAsync(int threadId, string userId)
        {
            try
            {
                var existingView = await _context.ThreadViews
                    .FirstOrDefaultAsync(tv => tv.ThreadId == threadId && tv.UserId == userId);

                if (existingView != null)
                {
                    // Update existing view timestamp
                    existingView.ViewedAt = DateTime.UtcNow;
                }
                else
                {
                    // Create new view
                    var view = new ThreadView
                    {
                        ThreadId = threadId,
                        UserId = userId,
                        ViewedAt = DateTime.UtcNow
                    };
                    _context.ThreadViews.Add(view);

                    // Update thread view count
                    var thread = await _context.CommunityThreads.FindAsync(threadId);
                    if (thread != null)
                    {
                        thread.ViewCount++;
                        thread.UpdatedAt = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing thread {ThreadId} by user {UserId}", 
                    threadId, userId);
                return false;
            }
        }

        public async Task<IEnumerable<CommunityThread>> SearchThreadsAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.CommunityThreads
                    .Include(ct => ct.Author)
                    .Include(ct => ct.Category)
                    .Where(ct => ct.IsActive && 
                        (ct.Title.Contains(searchTerm) || 
                         ct.Content.Contains(searchTerm) ||
                         ct.Tags.Any(t => t.Name.Contains(searchTerm))));

                return await query
                    .OrderByDescending(ct => ct.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching threads with term: {SearchTerm}", searchTerm);
                return new List<CommunityThread>();
            }
        }

        public async Task<IEnumerable<CommunityThread>> GetUserThreadsAsync(string userId, int page = 1, int pageSize = 20)
        {
            try
            {
                return await _context.CommunityThreads
                    .Include(ct => ct.Category)
                    .Where(ct => ct.AuthorId == userId && ct.IsActive)
                    .OrderByDescending(ct => ct.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting threads for user {UserId}", userId);
                return new List<CommunityThread>();
            }
        }

        public async Task<ThreadStatisticsViewModel> GetThreadStatisticsAsync(int threadId)
        {
            try
            {
                var thread = await _context.CommunityThreads
                    .Include(ct => ct.Author)
                    .FirstOrDefaultAsync(ct => ct.Id == threadId);

                if (thread == null)
                {
                    return new ThreadStatisticsViewModel();
                }

                var comments = await _context.ThreadComments
                    .Where(tc => tc.ThreadId == threadId && tc.IsActive)
                    .ToListAsync();

                var likes = await _context.ThreadLikes
                    .Where(tl => tl.ThreadId == threadId)
                    .ToListAsync();

                var views = await _context.ThreadViews
                    .Where(tv => tv.ThreadId == threadId)
                    .ToListAsync();

                return new ThreadStatisticsViewModel
                {
                    ThreadId = threadId,
                    Title = thread.Title,
                    AuthorName = thread.Author?.UserName ?? "Unknown User",
                    CreatedAt = thread.CreatedAt,
                    CommentCount = comments.Count,
                    LikeCount = likes.Count,
                    ViewCount = views.Count,
                    UniqueViewers = views.Select(v => v.UserId).Distinct().Count(),
                    LastActivity = thread.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics for thread {ThreadId}", threadId);
                return new ThreadStatisticsViewModel();
            }
        }

        private async Task LogUserActivityAsync(string userId, string category, string action, string details)
        {
            try
            {
                var activity = new UserActivity
                {
                    UserId = userId,
                    Category = category,
                    Action = action,
                    Details = details,
                    Timestamp = DateTime.UtcNow
                };

                _context.UserActivities.Add(activity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user activity for user {UserId}", userId);
            }
        }
    }
}
