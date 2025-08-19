using EzanaEzana.Models;
using EzanaEzana.Models.DashboardCards;
using EzanaEzana.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class DashboardCardsController : Controller
    {
        private readonly IDashboardCardsService _dashboardCardsService;
        private readonly IQuiverService _quiverService;
        private readonly ILogger<DashboardCardsController> _logger;

        public DashboardCardsController(
            IDashboardCardsService dashboardCardsService,
            IQuiverService quiverService,
            ILogger<DashboardCardsController> logger)
        {
            _dashboardCardsService = dashboardCardsService;
            _quiverService = quiverService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PortfolioValue()
        {
            return View();
        }

        #region Dashboard Cards API Endpoints
        [HttpGet("api/dashboard/portfolio-value")]
        public async Task<IActionResult> GetPortfolioValue([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var portfolioValue = await _dashboardCardsService.GetPortfolioValueCardAsync(userId, useMockData);
                return Json(portfolioValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get portfolio value data");
                return StatusCode(500, "Failed to retrieve portfolio value data");
            }
        }

        [HttpGet("api/dashboard/todays-pnl")]
        public async Task<IActionResult> GetTodaysPnl([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var pnlData = await _dashboardCardsService.GetTodaysPnlCardAsync(userId, useMockData);
                return Json(pnlData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get today's P&L data");
                return StatusCode(500, "Failed to retrieve today's P&L data");
            }
        }

        [HttpGet("api/dashboard/top-performer")]
        public async Task<IActionResult> GetTopPerformer()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var topPerformer = await _quiverService.GetTopPerformerAsync(userId);
                return Json(topPerformer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top performer data");
                return StatusCode(500, "Failed to retrieve top performer data");
            }
        }

        [HttpGet("api/dashboard/risk-score")]
        public async Task<IActionResult> GetRiskScore([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var riskScore = await _dashboardCardsService.GetRiskScoreCardAsync(userId, useMockData);
                return Json(riskScore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get risk score data");
                return StatusCode(500, "Failed to retrieve risk score data");
            }
        }

        [HttpGet("api/dashboard/monthly-dividends")]
        public async Task<IActionResult> GetMonthlyDividends([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var dividendIncome = await _dashboardCardsService.GetMonthlyDividendsCardAsync(userId, useMockData);
                return Json(dividendIncome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get monthly dividends data");
                return StatusCode(500, "Failed to retrieve monthly dividends data");
            }
        }

        [HttpGet("api/dashboard/top-performer")]
        public async Task<IActionResult> GetTopPerformer([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var topPerformer = await _dashboardCardsService.GetTopPerformerCardAsync(userId, useMockData);
                return Json(topPerformer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top performer data");
                return StatusCode(500, "Failed to retrieve top performer data");
            }
        }

        [HttpGet("api/dashboard/asset-allocation")]
        public async Task<IActionResult> GetAssetAllocation([FromQuery] string breakdownType = "asset_class", [FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var allocation = await _dashboardCardsService.GetAssetAllocationCardAsync(userId, breakdownType, useMockData);
                return Json(allocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get asset allocation data");
                return StatusCode(500, "Failed to retrieve asset allocation data");
            }
        }

        [HttpGet("api/dashboard/all-cards")]
        public async Task<IActionResult> GetAllDashboardCards([FromQuery] bool useMockData = false)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var allCardsData = await _dashboardCardsService.GetDashboardSummaryAsync(userId, useMockData);
                return Json(allCardsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all dashboard cards data");
                return StatusCode(500, "Failed to retrieve dashboard cards data");
            }
        }

        [HttpPost("api/dashboard/refresh")]
        public async Task<IActionResult> RefreshDashboardData()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var success = await _dashboardCardsService.RefreshFromPlaidAsync(userId);
                if (success)
                {
                    return Json(new { success = true, message = "Dashboard data refreshed successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to refresh dashboard data" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh dashboard data");
                return StatusCode(500, "Failed to refresh dashboard data");
            }
        }

        [HttpGet("api/dashboard/mock-data")]
        public IActionResult GetMockDashboardData()
        {
            try
            {
                var mockData = _dashboardCardsService.GetMockDashboardData();
                return Json(mockData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get mock dashboard data");
                return StatusCode(500, "Failed to retrieve mock dashboard data");
            }
        }
        #endregion

        #region Private Helper Methods
        private string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
        #endregion
    }
}
