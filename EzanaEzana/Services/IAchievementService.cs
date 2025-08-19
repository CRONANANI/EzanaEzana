using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzanaEzana.Services
{
    public interface IAchievementService
    {
        // Achievement Retrieval
        Task<IEnumerable<Achievement>> GetAllAchievementsAsync();
        Task<Achievement?> GetAchievementByIdAsync(int id);
        Task<IEnumerable<Achievement>> GetAchievementsByCategoryAsync(string category);
        Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(string userId);
        Task<IEnumerable<UserAchievement>> GetRecentUserAchievementsAsync(string userId, int count = 5);

        // Achievement Awarding
        Task<bool> AwardAchievementToUserAsync(string userId, int achievementId, string? notes = null);
        Task<bool> CheckAndAwardInvestmentAchievementsAsync(string userId);
        Task<bool> CheckAndAwardCommunityAchievementsAsync(string userId);
        Task<bool> CheckAndAwardLearningAchievementsAsync(string userId);

        // Trophy Room and Statistics
        Task<TrophyRoomViewModel> GetTrophyRoomAsync(string userId);
        Task<AchievementStatisticsViewModel> GetUserStatisticsAsync(string userId);
        Task<IEnumerable<LeaderboardEntryViewModel>> GetLeaderboardAsync(int count = 100);

        // User Activity Logging
        Task LogUserActivityAsync(string userId, string category, string action, string details);
    }
}
