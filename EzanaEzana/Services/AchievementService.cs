using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AchievementService> _logger;

        public AchievementService(ApplicationDbContext context, ILogger<AchievementService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Achievement>> GetAllAchievementsAsync()
        {
            return await _context.Achievements
                .Where(a => a.IsActive)
                .OrderBy(a => a.Category)
                .ThenBy(a => a.Points)
                .ToListAsync();
        }

        public async Task<Achievement?> GetAchievementByIdAsync(int id)
        {
            return await _context.Achievements
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        }

        public async Task<IEnumerable<Achievement>> GetAchievementsByCategoryAsync(string category)
        {
            return await _context.Achievements
                .Where(a => a.Category == category && a.IsActive)
                .OrderBy(a => a.Points)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(string userId)
        {
            return await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.EarnedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAchievement>> GetRecentUserAchievementsAsync(string userId, int count = 5)
        {
            return await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.EarnedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> AwardAchievementToUserAsync(string userId, int achievementId, string? notes = null)
        {
            try
            {
                // Check if user already has this achievement
                var existingAchievement = await _context.UserAchievements
                    .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);

                if (existingAchievement != null)
                {
                    return false; // User already has this achievement
                }

                var achievement = await _context.Achievements.FindAsync(achievementId);
                if (achievement == null || !achievement.IsActive)
                {
                    return false;
                }

                var userAchievement = new UserAchievement
                {
                    UserId = userId,
                    AchievementId = achievementId,
                    EarnedAt = DateTime.UtcNow,
                    Notes = notes
                };

                _context.UserAchievements.Add(userAchievement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Achievement {AchievementName} awarded to user {UserId}", 
                    achievement.Name, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error awarding achievement {AchievementId} to user {UserId}", 
                    achievementId, userId);
                return false;
            }
        }

        public async Task<bool> CheckAndAwardInvestmentAchievementsAsync(string userId)
        {
            try
            {
                // Get user's investment statistics
                var portfolioStats = await _context.PortfolioHoldings
                    .Where(ph => ph.Portfolio.UserId == userId)
                    .GroupBy(ph => ph.Portfolio.UserId)
                    .Select(g => new
                    {
                        TotalHoldings = g.Count(),
                        TotalValue = g.Sum(ph => ph.CurrentValue),
                        UniqueStocks = g.Select(ph => ph.Symbol).Distinct().Count()
                    })
                    .FirstOrDefaultAsync();

                if (portfolioStats == null) return false;

                var achievements = await _context.Achievements
                    .Where(a => a.Category == "Investment" && a.IsActive)
                    .ToListAsync();

                var awarded = false;

                foreach (var achievement in achievements)
                {
                    bool shouldAward = false;

                    switch (achievement.Type)
                    {
                        case "FirstStock":
                            if (portfolioStats.TotalHoldings >= 1)
                                shouldAward = true;
                            break;
                        case "StockDiversifier":
                            if (portfolioStats.UniqueStocks >= 5)
                                shouldAward = true;
                            break;
                        case "PortfolioBuilder":
                            if (portfolioStats.TotalHoldings >= 10)
                                shouldAward = true;
                            break;
                        case "ValueInvestor":
                            if (portfolioStats.TotalValue >= 10000)
                                shouldAward = true;
                            break;
                        case "Millionaire":
                            if (portfolioStats.TotalValue >= 1000000)
                                shouldAward = true;
                            break;
                    }

                    if (shouldAward)
                    {
                        var success = await AwardAchievementToUserAsync(userId, achievement.Id);
                        if (success) awarded = true;
                    }
                }

                return awarded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking investment achievements for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CheckAndAwardCommunityAchievementsAsync(string userId)
        {
            try
            {
                // Get user's community activity statistics
                var communityStats = await _context.CommunityThreads
                    .Where(ct => ct.AuthorId == userId)
                    .GroupBy(ct => ct.AuthorId)
                    .Select(g => new
                    {
                        ThreadCount = g.Count(),
                        TotalLikes = g.Sum(ct => ct.LikeCount),
                        TotalViews = g.Sum(ct => ct.ViewCount)
                    })
                    .FirstOrDefaultAsync();

                var commentStats = await _context.ThreadComments
                    .Where(tc => tc.AuthorId == userId)
                    .GroupBy(tc => tc.AuthorId)
                    .Select(g => new
                    {
                        CommentCount = g.Count(),
                        TotalLikes = g.Sum(tc => tc.LikeCount)
                    })
                    .FirstOrDefaultAsync();

                var achievements = await _context.Achievements
                    .Where(a => a.Category == "Community" && a.IsActive)
                    .ToListAsync();

                var awarded = false;

                foreach (var achievement in achievements)
                {
                    bool shouldAward = false;

                    switch (achievement.Type)
                    {
                        case "FirstPost":
                            if (communityStats?.ThreadCount >= 1)
                                shouldAward = true;
                            break;
                        case "ActivePoster":
                            if (communityStats?.ThreadCount >= 10)
                                shouldAward = true;
                            break;
                        case "PopularPoster":
                            if (communityStats?.TotalLikes >= 50)
                                shouldAward = true;
                            break;
                        case "FirstComment":
                            if (commentStats?.CommentCount >= 1)
                                shouldAward = true;
                            break;
                        case "HelpfulCommenter":
                            if (commentStats?.TotalLikes >= 25)
                                shouldAward = true;
                            break;
                        case "CommunityLeader":
                            if (communityStats?.TotalViews >= 1000)
                                shouldAward = true;
                            break;
                    }

                    if (shouldAward)
                    {
                        var success = await AwardAchievementToUserAsync(userId, achievement.Id);
                        if (success) awarded = true;
                    }
                }

                return awarded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking community achievements for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CheckAndAwardLearningAchievementsAsync(string userId)
        {
            try
            {
                // Get user's learning activity statistics
                var learningStats = await _context.UserActivities
                    .Where(ua => ua.UserId == userId && ua.Category == "Learning")
                    .GroupBy(ua => ua.UserId)
                    .Select(g => new
                    {
                        ActivityCount = g.Count(),
                        LastActivity = g.Max(ua => ua.Timestamp)
                    })
                    .FirstOrDefaultAsync();

                var achievements = await _context.Achievements
                    .Where(a => a.Category == "Learning" && a.IsActive)
                    .ToListAsync();

                var awarded = false;

                foreach (var achievement in achievements)
                {
                    bool shouldAward = false;

                    switch (achievement.Type)
                    {
                        case "FirstResearch":
                            if (learningStats?.ActivityCount >= 1)
                                shouldAward = true;
                            break;
                        case "ResearchEnthusiast":
                            if (learningStats?.ActivityCount >= 20)
                                shouldAward = true;
                            break;
                        case "MarketAnalyst":
                            if (learningStats?.ActivityCount >= 50)
                                shouldAward = true;
                            break;
                        case "WeeklyLearner":
                            if (learningStats?.LastActivity >= DateTime.UtcNow.AddDays(-7))
                                shouldAward = true;
                            break;
                    }

                    if (shouldAward)
                    {
                        var success = await AwardAchievementToUserAsync(userId, achievement.Id);
                        if (success) awarded = true;
                    }
                }

                return awarded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking learning achievements for user {UserId}", userId);
                return false;
            }
        }

        public async Task<TrophyRoomViewModel> GetTrophyRoomDataAsync(string userId)
        {
            try
            {
                var userAchievements = await _context.UserAchievements
                    .Include(ua => ua.Achievement)
                    .Where(ua => ua.UserId == userId)
                    .OrderByDescending(ua => ua.EarnedAt)
                    .ToListAsync();

                var totalPoints = userAchievements.Sum(ua => ua.Achievement.Points);
                var achievementCount = userAchievements.Count;
                var categories = userAchievements
                    .Select(ua => ua.Achievement.Category)
                    .Distinct()
                    .ToList();

                var recentAchievements = userAchievements.Take(5).ToList();

                return new TrophyRoomViewModel
                {
                    TotalPoints = totalPoints,
                    AchievementCount = achievementCount,
                    Categories = categories,
                    RecentAchievements = recentAchievements.Select(ua => new AchievementSummaryViewModel
                    {
                        Id = ua.Achievement.Id,
                        Name = ua.Achievement.Name,
                        Description = ua.Achievement.Description,
                        IconClass = ua.Achievement.IconClass,
                        ColorScheme = ua.Achievement.ColorScheme,
                        Points = ua.Achievement.Points,
                        EarnedAt = ua.EarnedAt
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trophy room data for user {UserId}", userId);
                return new TrophyRoomViewModel();
            }
        }

        public async Task<UserStatisticsViewModel> GetUserStatisticsAsync(string userId)
        {
            try
            {
                var userAchievements = await _context.UserAchievements
                    .Include(ua => ua.Achievement)
                    .Where(ua => ua.UserId == userId)
                    .ToListAsync();

                var stats = new UserStatisticsViewModel
                {
                    TotalPoints = userAchievements.Sum(ua => ua.Achievement.Points),
                    AchievementCount = userAchievements.Count,
                    CategoryBreakdown = userAchievements
                        .GroupBy(ua => ua.Achievement.Category)
                        .Select(g => new CategoryBreakdownViewModel
                        {
                            Category = g.Key,
                            Count = g.Count(),
                            Points = g.Sum(ua => ua.Achievement.Points)
                        })
                        .OrderByDescending(cb => cb.Points)
                        .ToList(),
                    RecentAchievements = userAchievements
                        .OrderByDescending(ua => ua.EarnedAt)
                        .Take(10)
                        .Select(ua => new AchievementSummaryViewModel
                        {
                            Id = ua.Achievement.Id,
                            Name = ua.Achievement.Name,
                            Description = ua.Achievement.Description,
                            IconClass = ua.Achievement.IconClass,
                            ColorScheme = ua.Achievement.ColorScheme,
                            Points = ua.Achievement.Points,
                            EarnedAt = ua.EarnedAt
                        })
                        .ToList()
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user statistics for user {UserId}", userId);
                return new UserStatisticsViewModel();
            }
        }

        public async Task<IEnumerable<LeaderboardEntryViewModel>> GetLeaderboardAsync(int count = 20)
        {
            try
            {
                var leaderboard = await _context.UserAchievements
                    .GroupBy(ua => ua.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TotalPoints = g.Sum(ua => ua.Achievement.Points),
                        AchievementCount = g.Count()
                    })
                    .OrderByDescending(entry => entry.TotalPoints)
                    .ThenByDescending(entry => entry.AchievementCount)
                    .Take(count)
                    .ToListAsync();

                var result = new List<LeaderboardEntryViewModel>();

                for (int i = 0; i < leaderboard.Count; i++)
                {
                    var entry = leaderboard[i];
                    var user = await _context.Users.FindAsync(entry.UserId);

                    result.Add(new LeaderboardEntryViewModel
                    {
                        Rank = i + 1,
                        UserId = entry.UserId,
                        Username = user?.UserName ?? "Unknown User",
                        TotalPoints = entry.TotalPoints,
                        AchievementCount = entry.AchievementCount
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leaderboard");
                return new List<LeaderboardEntryViewModel>();
            }
        }

        public async Task<IEnumerable<Achievement>> GetAvailableAchievementsAsync(string userId)
        {
            try
            {
                var earnedAchievementIds = await _context.UserAchievements
                    .Where(ua => ua.UserId == userId)
                    .Select(ua => ua.AchievementId)
                    .ToListAsync();

                return await _context.Achievements
                    .Where(a => a.IsActive && !earnedAchievementIds.Contains(a.Id))
                    .OrderBy(a => a.Category)
                    .ThenBy(a => a.Points)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available achievements for user {UserId}", userId);
                return new List<Achievement>();
            }
        }

        public async Task<bool> IsAchievementEarnedAsync(string userId, int achievementId)
        {
            try
            {
                return await _context.UserAchievements
                    .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if achievement {AchievementId} is earned by user {UserId}", 
                    achievementId, userId);
                return false;
            }
        }
    }
}
