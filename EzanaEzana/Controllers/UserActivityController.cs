using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class UserActivityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserActivityController> _logger;

        public UserActivityController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<UserActivityController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: UserActivity
        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => ua.UserId == userId)
                    .OrderByDescending(ua => ua.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalActivities = await _context.UserActivities
                    .Where(ua => ua.UserId == userId)
                    .CountAsync();

                var viewModel = new UserActivityIndexViewModel
                {
                    Activities = activities.Select(ua => new UserActivityViewModel
                    {
                        Id = ua.Id,
                        Category = ua.Category,
                        Action = ua.Action,
                        Details = ua.Details,
                        Timestamp = ua.Timestamp,
                        UserName = ua.User?.UserName ?? "Unknown User"
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalActivities = totalActivities,
                    TotalPages = (int)Math.Ceiling((double)totalActivities / pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user activities");
                return View("Error");
            }
        }

        // GET: UserActivity/Friends
        public async Task<IActionResult> Friends(int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Get user's friends
                var friends = await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == FriendshipStatus.Accepted)
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .ToListAsync();

                if (!friends.Any())
                {
                    return View(new FriendsActivityViewModel
                    {
                        Activities = new List<UserActivityViewModel>(),
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalActivities = 0,
                        TotalPages = 0
                    });
                }

                // Get friends' activities
                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => friends.Contains(ua.UserId))
                    .OrderByDescending(ua => ua.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalActivities = await _context.UserActivities
                    .Where(ua => friends.Contains(ua.UserId))
                    .CountAsync();

                var viewModel = new FriendsActivityViewModel
                {
                    Activities = activities.Select(ua => new UserActivityViewModel
                    {
                        Id = ua.Id,
                        Category = ua.Category,
                        Action = ua.Action,
                        Details = ua.Details,
                        Timestamp = ua.Timestamp,
                        UserName = ua.User?.UserName ?? "Unknown User"
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalActivities = totalActivities,
                    TotalPages = (int)Math.Ceiling((double)totalActivities / pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading friends activities");
                return View("Error");
            }
        }

        // GET: UserActivity/Community
        public async Task<IActionResult> Community(int page = 1, int pageSize = 20)
        {
            try
            {
                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => ua.Category == "Community")
                    .OrderByDescending(ua => ua.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalActivities = await _context.UserActivities
                    .Where(ua => ua.Category == "Community")
                    .CountAsync();

                var viewModel = new CommunityActivityViewModel
                {
                    Activities = activities.Select(ua => new UserActivityViewModel
                    {
                        Id = ua.Id,
                        Category = ua.Category,
                        Action = ua.Action,
                        Details = ua.Details,
                        Timestamp = ua.Timestamp,
                        UserName = ua.User?.UserName ?? "Unknown User"
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalActivities = totalActivities,
                    TotalPages = (int)Math.Ceiling((double)totalActivities / pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading community activities");
                return View("Error");
            }
        }

        // GET: UserActivity/Investment
        public async Task<IActionResult> Investment(int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => ua.Category == "Portfolio" || ua.Category == "Investment")
                    .OrderByDescending(ua => ua.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalActivities = await _context.UserActivities
                    .Where(ua => ua.Category == "Portfolio" || ua.Category == "Investment")
                    .CountAsync();

                var viewModel = new InvestmentActivityViewModel
                {
                    Activities = activities.Select(ua => new UserActivityViewModel
                    {
                        Id = ua.Id,
                        Category = ua.Category,
                        Action = ua.Action,
                        Details = ua.Details,
                        Timestamp = ua.Timestamp,
                        UserName = ua.User?.UserName ?? "Unknown User"
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalActivities = totalActivities,
                    TotalPages = (int)Math.Ceiling((double)totalActivities / pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading investment activities");
                return View("Error");
            }
        }

        // GET: UserActivity/Statistics
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var userActivities = await _context.UserActivities
                    .Where(ua => ua.UserId == userId)
                    .ToListAsync();

                var totalActivities = userActivities.Count;
                var activitiesThisWeek = userActivities.Count(ua => ua.Timestamp >= DateTime.UtcNow.AddDays(-7));
                var activitiesThisMonth = userActivities.Count(ua => ua.Timestamp >= DateTime.UtcNow.AddDays(-30));

                var categoryBreakdown = userActivities
                    .GroupBy(ua => ua.Category)
                    .Select(g => new ActivityCategoryViewModel
                    {
                        Category = g.Key,
                        Count = g.Count(),
                        Percentage = totalActivities > 0 ? (double)g.Count() / totalActivities * 100 : 0
                    })
                    .OrderByDescending(ac => ac.Count)
                    .ToList();

                var recentActivityTrend = await GetRecentActivityTrendAsync(userId);

                var viewModel = new UserActivityStatisticsViewModel
                {
                    TotalActivities = totalActivities,
                    ActivitiesThisWeek = activitiesThisWeek,
                    ActivitiesThisMonth = activitiesThisMonth,
                    CategoryBreakdown = categoryBreakdown,
                    RecentActivityTrend = recentActivityTrend
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user activity statistics");
                return View("Error");
            }
        }

        // GET: UserActivity/Feed
        public async Task<IActionResult> Feed(int page = 1, int pageSize = 50)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Get user's friends
                var friends = await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == FriendshipStatus.Accepted)
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .ToListAsync();

                // Get activities from user and friends
                var allUserIds = new List<string> { userId };
                allUserIds.AddRange(friends);

                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => allUserIds.Contains(ua.UserId))
                    .OrderByDescending(ua => ua.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalActivities = await _context.UserActivities
                    .Where(ua => allUserIds.Contains(ua.UserId))
                    .CountAsync();

                var viewModel = new ActivityFeedViewModel
                {
                    Activities = activities.Select(ua => new UserActivityViewModel
                    {
                        Id = ua.Id,
                        Category = ua.Category,
                        Action = ua.Action,
                        Details = ua.Details,
                        Timestamp = ua.Timestamp,
                        UserName = ua.User?.UserName ?? "Unknown User",
                        IsCurrentUser = ua.UserId == userId
                    }).ToList(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalActivities = totalActivities,
                    TotalPages = (int)Math.Ceiling((double)totalActivities / pageSize)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading activity feed");
                return View("Error");
            }
        }

        // POST: UserActivity/Log
        [HttpPost]
        public async Task<IActionResult> Log([FromBody] LogActivityRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var activity = new UserActivity
                {
                    UserId = userId,
                    Category = request.Category,
                    Action = request.Action,
                    Details = request.Details,
                    Timestamp = DateTime.UtcNow
                };

                _context.UserActivities.Add(activity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Activity logged for user {UserId}: {Category} - {Action}", 
                    userId, request.Category, request.Action);

                return Ok(new { message = "Activity logged successfully", activityId = activity.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging activity");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: UserActivity/API/Recent
        [HttpGet("API/Recent")]
        public async Task<IActionResult> GetRecentActivities(int count = 10)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => ua.UserId == userId)
                    .OrderByDescending(ua => ua.Timestamp)
                    .Take(count)
                    .Select(ua => new
                    {
                        ua.Id,
                        ua.Category,
                        ua.Action,
                        ua.Details,
                        ua.Timestamp,
                        UserName = ua.User.UserName
                    })
                    .ToListAsync();

                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent activities via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: UserActivity/API/Friends
        [HttpGet("API/Friends")]
        public async Task<IActionResult> GetFriendsActivities(int count = 20)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var friends = await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == FriendshipStatus.Accepted)
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .ToListAsync();

                if (!friends.Any())
                {
                    return Ok(new List<object>());
                }

                var activities = await _context.UserActivities
                    .Include(ua => ua.User)
                    .Where(ua => friends.Contains(ua.UserId))
                    .OrderByDescending(ua => ua.Timestamp)
                    .Take(count)
                    .Select(ua => new
                    {
                        ua.Id,
                        ua.Category,
                        ua.Action,
                        ua.Details,
                        ua.Timestamp,
                        UserName = ua.User.UserName
                    })
                    .ToListAsync();

                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting friends activities via API");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<DailyActivityViewModel>> GetRecentActivityTrendAsync(string userId)
        {
            try
            {
                var endDate = DateTime.UtcNow.Date;
                var startDate = endDate.AddDays(-29); // Last 30 days

                var activities = await _context.UserActivities
                    .Where(ua => ua.UserId == userId && 
                                ua.Timestamp >= startDate && 
                                ua.Timestamp <= endDate)
                    .ToListAsync();

                var dailyActivities = new List<DailyActivityViewModel>();
                var currentDate = startDate;

                while (currentDate <= endDate)
                {
                    var dayActivities = activities.Count(ua => ua.Timestamp.Date == currentDate);
                    
                    dailyActivities.Add(new DailyActivityViewModel
                    {
                        Date = currentDate,
                        ActivityCount = dayActivities
                    });

                    currentDate = currentDate.AddDays(1);
                }

                return dailyActivities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent activity trend for user {UserId}", userId);
                return new List<DailyActivityViewModel>();
            }
        }
    }
}
