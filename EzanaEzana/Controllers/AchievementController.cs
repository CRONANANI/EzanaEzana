using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class AchievementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAchievementService _achievementService;

        public AchievementController(
            UserManager<ApplicationUser> userManager,
            IAchievementService achievementService)
        {
            _userManager = userManager;
            _achievementService = achievementService;
        }

        // GET: /Achievement/TrophyRoom
        public async Task<IActionResult> TrophyRoom()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var trophyRoom = await _achievementService.GetUserTrophyRoomAsync(user.Id);
                return View(trophyRoom);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Achievement/Statistics
        public async Task<IActionResult> Statistics()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var statistics = await _achievementService.GetAchievementStatisticsAsync(user.Id);
                return View(statistics);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Achievement/Leaderboard
        public async Task<IActionResult> Leaderboard(int count = 10)
        {
            try
            {
                var leaderboard = await _achievementService.GetAchievementLeaderboardAsync(count);
                return View(leaderboard);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Achievement/All
        public async Task<IActionResult> All()
        {
            try
            {
                var achievements = await _achievementService.GetAllAchievementsAsync();
                return View(achievements);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Achievement/Category/{category}
        public async Task<IActionResult> Category(string category)
        {
            try
            {
                var achievements = await _achievementService.GetAchievementsByCategoryAsync(category);
                return View(achievements);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Achievement/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var achievement = await _achievementService.GetAchievementByIdAsync(id);
                if (achievement == null)
                {
                    return NotFound();
                }

                return View(achievement);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: /Achievement/CheckInvestment
        [HttpPost]
        public async Task<IActionResult> CheckInvestmentAchievements()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                await _achievementService.CheckAndAwardInvestmentAchievementsAsync(user.Id);
                return Json(new { success = true, message = "Investment achievements checked successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Achievement/CheckCommunity
        [HttpPost]
        public async Task<IActionResult> CheckCommunityAchievements()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                await _achievementService.CheckAndAwardCommunityAchievementsAsync(user.Id);
                return Json(new { success = true, message = "Community achievements checked successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Achievement/CheckLearning
        [HttpPost]
        public async Task<IActionResult> CheckLearningAchievements()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                await _achievementService.CheckAndAwardLearningAchievementsAsync(user.Id);
                return Json(new { success = true, message = "Learning achievements checked successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Achievement/API/UserAchievements
        [HttpGet]
        [Route("api/achievement/user/{userId}")]
        public async Task<IActionResult> GetUserAchievements(string userId)
        {
            try
            {
                var achievements = await _achievementService.GetUserAchievementsAsync(userId);
                return Json(achievements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Achievement/API/Recent
        [HttpGet]
        [Route("api/achievement/recent/{userId}")]
        public async Task<IActionResult> GetRecentAchievements(string userId, int count = 5)
        {
            try
            {
                var achievements = await _achievementService.GetRecentAchievementsAsync(userId, count);
                return Json(achievements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Achievement/API/Statistics
        [HttpGet]
        [Route("api/achievement/statistics/{userId}")]
        public async Task<IActionResult> GetAchievementStatistics(string userId)
        {
            try
            {
                var statistics = await _achievementService.GetAchievementStatisticsAsync(userId);
                return Json(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Achievement/API/Leaderboard
        [HttpGet]
        [Route("api/achievement/leaderboard")]
        public async Task<IActionResult> GetLeaderboard(int count = 10)
        {
            try
            {
                var leaderboard = await _achievementService.GetAchievementLeaderboardAsync(count);
                return Json(leaderboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
