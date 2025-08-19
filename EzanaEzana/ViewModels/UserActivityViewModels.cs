using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Request/Response Models
    public class LogActivityRequest
    {
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    // View Models
    public class UserActivityIndexViewModel
    {
        public List<UserActivityViewModel> Activities { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalActivities { get; set; }
        public int TotalPages { get; set; }
    }

    public class FriendsActivityViewModel
    {
        public List<UserActivityViewModel> Activities { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalActivities { get; set; }
        public int TotalPages { get; set; }
    }

    public class CommunityActivityViewModel
    {
        public List<UserActivityViewModel> Activities { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalActivities { get; set; }
        public int TotalPages { get; set; }
    }

    public class InvestmentActivityViewModel
    {
        public List<UserActivityViewModel> Activities { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalActivities { get; set; }
        public int TotalPages { get; set; }
    }

    public class ActivityFeedViewModel
    {
        public List<UserActivityViewModel> Activities { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalActivities { get; set; }
        public int TotalPages { get; set; }
    }

    public class UserActivityViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool IsCurrentUser { get; set; }
    }

    public class UserActivityStatisticsViewModel
    {
        public int TotalActivities { get; set; }
        public int ActivitiesThisWeek { get; set; }
        public int ActivitiesThisMonth { get; set; }
        public List<ActivityCategoryViewModel> CategoryBreakdown { get; set; } = new();
        public List<DailyActivityViewModel> RecentActivityTrend { get; set; } = new();
    }

    public class ActivityCategoryViewModel
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class DailyActivityViewModel
    {
        public DateTime Date { get; set; }
        public int ActivityCount { get; set; }
    }
}
