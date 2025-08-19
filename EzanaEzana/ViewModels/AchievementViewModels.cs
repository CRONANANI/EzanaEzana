using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Achievement ViewModels
    public class TrophyRoomViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int TotalAchievements { get; set; }
        public int TotalPoints { get; set; }
        public int Rank { get; set; }
        public List<AchievementViewModel> RecentAchievements { get; set; } = new List<AchievementViewModel>();
        public List<AchievementViewModel> AllAchievements { get; set; } = new List<AchievementViewModel>();
        public List<AchievementCategoryViewModel> Categories { get; set; } = new List<AchievementCategoryViewModel>();
        public AchievementStatisticsViewModel Statistics { get; set; } = new AchievementStatisticsViewModel();
    }

    public class AchievementCategoryViewModel
    {
        public string Category { get; set; } = string.Empty;
        public int TotalAchievements { get; set; }
        public int EarnedAchievements { get; set; }
        public int TotalPoints { get; set; }
        public int EarnedPoints { get; set; }
        public List<AchievementViewModel> Achievements { get; set; } = new List<AchievementViewModel>();
        
        // Computed Properties
        public int RemainingAchievements => TotalAchievements - EarnedAchievements;
        public int RemainingPoints => TotalPoints - EarnedPoints;
        public decimal CompletionPercentage => TotalAchievements > 0 ? (decimal)EarnedAchievements / TotalAchievements * 100 : 0;
    }

    public class AchievementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string ColorScheme { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsEarned { get; set; }
        public DateTime? EarnedAt { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Computed Properties
        public string StatusText => IsEarned ? "Earned" : "Not Earned";
        public string StatusColor => IsEarned ? "text-green-600" : "text-gray-500";
        public string TimeAgo
        {
            get
            {
                if (!EarnedAt.HasValue) return "Not earned";
                var timeSpan = DateTime.UtcNow - EarnedAt.Value;
                if (timeSpan.TotalMinutes < 1) return "Just earned";
                if (timeSpan.TotalMinutes < 60) return $"Earned {(int)timeSpan.TotalMinutes}m ago";
                if (timeSpan.TotalHours < 24) return $"Earned {(int)timeSpan.TotalHours}h ago";
                if (timeSpan.TotalDays < 7) return $"Earned {(int)timeSpan.TotalDays}d ago";
                return $"Earned {EarnedAt.Value.ToString("MMM dd")}";
            }
        }
    }

    public class AchievementStatisticsViewModel
    {
        public int TotalAchievements { get; set; }
        public int EarnedAchievements { get; set; }
        public int TotalPoints { get; set; }
        public int EarnedPoints { get; set; }
        public int Rank { get; set; }
        public int TotalUsers { get; set; }
        public List<MonthlyAchievementViewModel> MonthlyProgress { get; set; } = new List<MonthlyAchievementViewModel>();
        public List<AchievementProgressViewModel> CategoryProgress { get; set; } = new List<AchievementProgressViewModel>();
        public List<LeaderboardEntryViewModel> TopUsers { get; set; } = new List<LeaderboardEntryViewModel>();
        
        // Computed Properties
        public int RemainingAchievements => TotalAchievements - EarnedAchievements;
        public int RemainingPoints => TotalPoints - EarnedPoints;
        public decimal CompletionPercentage => TotalAchievements > 0 ? (decimal)EarnedAchievements / TotalAchievements * 100 : 0;
        public decimal PointsPercentage => TotalPoints > 0 ? (decimal)EarnedPoints / TotalPoints * 100 : 0;
        public bool IsInTopTen => Rank <= 10;
        public bool IsInTopHundred => Rank <= 100;
    }

    public class MonthlyAchievementViewModel
    {
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public int AchievementsEarned { get; set; }
        public int PointsEarned { get; set; }
        public List<AchievementViewModel> Achievements { get; set; } = new List<AchievementViewModel>();
    }

    public class AchievementProgressViewModel
    {
        public string Category { get; set; } = string.Empty;
        public int TotalAchievements { get; set; }
        public int EarnedAchievements { get; set; }
        public int TotalPoints { get; set; }
        public int EarnedPoints { get; set; }
        
        // Computed Properties
        public int RemainingAchievements => TotalAchievements - EarnedAchievements;
        public int RemainingPoints => TotalPoints - EarnedPoints;
        public decimal CompletionPercentage => TotalAchievements > 0 ? (decimal)EarnedAchievements / TotalAchievements * 100 : 0;
        public decimal PointsPercentage => TotalPoints > 0 ? (decimal)EarnedPoints / TotalPoints * 100 : 0;
    }

    public class LeaderboardEntryViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Rank { get; set; }
        public int TotalAchievements { get; set; }
        public int TotalPoints { get; set; }
        public DateTime LastAchievement { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public string RankText => Rank switch
        {
            1 => "🥇 1st",
            2 => "🥈 2nd",
            3 => "🥉 3rd",
            _ => $"#{Rank}"
        };
    }

    public class CreateAchievementViewModel
    {
        [Required(ErrorMessage = "Achievement name is required")]
        [StringLength(100, ErrorMessage = "Achievement name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement description is required")]
        [StringLength(500, ErrorMessage = "Achievement description cannot be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement category is required")]
        [StringLength(50, ErrorMessage = "Achievement category cannot be longer than 50 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement type is required")]
        [StringLength(50, ErrorMessage = "Achievement type cannot be longer than 50 characters")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Icon class is required")]
        [StringLength(100, ErrorMessage = "Icon class cannot be longer than 100 characters")]
        public string IconClass { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color scheme is required")]
        [StringLength(50, ErrorMessage = "Color scheme cannot be longer than 50 characters")]
        public string ColorScheme { get; set; } = string.Empty;

        [Required(ErrorMessage = "Points are required")]
        [Range(1, 1000, ErrorMessage = "Points must be between 1 and 1000")]
        public int Points { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class UpdateAchievementViewModel
    {
        [Required(ErrorMessage = "Achievement name is required")]
        [StringLength(100, ErrorMessage = "Achievement name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement description is required")]
        [StringLength(500, ErrorMessage = "Achievement description cannot be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement category is required")]
        [StringLength(50, ErrorMessage = "Achievement category cannot be longer than 50 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achievement type is required")]
        [StringLength(50, ErrorMessage = "Achievement type cannot be longer than 50 characters")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Icon class is required")]
        [StringLength(100, ErrorMessage = "Icon class cannot be longer than 100 characters")]
        public string IconClass { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color scheme is required")]
        [StringLength(50, ErrorMessage = "Color scheme cannot be longer than 50 characters")]
        public string ColorScheme { get; set; } = string.Empty;

        [Required(ErrorMessage = "Points are required")]
        [Range(1, 1000, ErrorMessage = "Points must be between 1 and 1000")]
        public int Points { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }

    public class UserStatisticsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int TotalAchievements { get; set; }
        public int EarnedAchievements { get; set; }
        public int TotalPoints { get; set; }
        public int EarnedPoints { get; set; }
        public int Rank { get; set; }
        public int TotalUsers { get; set; }
        public DateTime FirstAchievement { get; set; }
        public DateTime LastAchievement { get; set; }
        public List<AchievementViewModel> RecentAchievements { get; set; } = new List<AchievementViewModel>();
        public List<AchievementProgressViewModel> CategoryProgress { get; set; } = new List<AchievementProgressViewModel>();
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FirstName : UserName;
        public int RemainingAchievements => TotalAchievements - EarnedAchievements;
        public int RemainingPoints => TotalPoints - EarnedPoints;
        public decimal CompletionPercentage => TotalAchievements > 0 ? (decimal)EarnedAchievements / TotalAchievements * 100 : 0;
        public decimal PointsPercentage => TotalPoints > 0 ? (decimal)EarnedPoints / TotalPoints * 100 : 0;
        public bool IsInTopTen => Rank <= 10;
        public bool IsInTopHundred => Rank <= 100;
        public TimeSpan TimeSinceFirstAchievement => DateTime.UtcNow - FirstAchievement;
        public TimeSpan TimeSinceLastAchievement => DateTime.UtcNow - LastAchievement;
    }
}
