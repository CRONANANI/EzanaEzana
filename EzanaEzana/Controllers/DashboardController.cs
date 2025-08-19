using EzanaEzana.Models;
using EzanaEzana.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IQuiverService _quiverService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IQuiverService quiverService,
            ILogger<DashboardController> logger)
        {
            _quiverService = quiverService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("portfolio-dashboard")]
        public IActionResult PortfolioDashboard()
        {
            return View();
        }

        [HttpGet("investment-preferences")]
        public IActionResult InvestmentPreferences()
        {
            return View();
        }

        [HttpGet("market-intelligence")]
        public IActionResult MarketIntelligence()
        {
            return View();
        }

        [HttpGet("test-quiver")]
        public IActionResult TestQuiver()
        {
            return View();
        }

        #region API Endpoints for Frontend
        [HttpGet("api/portfolio/summary")]
        public async Task<IActionResult> GetPortfolioSummary()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var summary = await _quiverService.GetPortfolioSummaryAsync(userId);
                return Json(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get portfolio summary");
                return StatusCode(500, "Failed to retrieve portfolio summary");
            }
        }

        [HttpGet("api/portfolio/asset-allocation")]
        public async Task<IActionResult> GetAssetAllocation([FromQuery] string breakdownType = "asset_class")
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var allocation = await _quiverService.GetAssetAllocationAsync(userId, breakdownType);
                return Json(allocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get asset allocation");
                return StatusCode(500, "Failed to retrieve asset allocation");
            }
        }

        [HttpGet("api/portfolio/top-holdings")]
        public async Task<IActionResult> GetTopHoldings([FromQuery] int count = 3)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var holdings = await _quiverService.GetTopHoldingsAsync(userId, count);
                return Json(holdings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top holdings");
                return StatusCode(500, "Failed to retrieve top holdings");
            }
        }

        [HttpGet("api/portfolio/history")]
        public async Task<IActionResult> GetPortfolioHistory([FromQuery] int months = 12)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var history = await _quiverService.GetPortfolioHistoryAsync(userId, months);
                return Json(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get portfolio history");
                return StatusCode(500, "Failed to retrieve portfolio history");
            }
        }

        [HttpGet("api/portfolio/risk-score")]
        public async Task<IActionResult> GetRiskScore()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var riskScore = await _quiverService.GetRiskScoreAsync(userId);
                return Json(riskScore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get risk score");
                return StatusCode(500, "Failed to retrieve risk score");
            }
        }

        [HttpGet("api/portfolio/dividend-income")]
        public async Task<IActionResult> GetDividendIncome()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var dividendIncome = await _quiverService.GetDividendIncomeAsync(userId);
                return Json(dividendIncome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get dividend income");
                return StatusCode(500, "Failed to retrieve dividend income");
            }
        }

        [HttpGet("api/portfolio/top-performer")]
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
                _logger.LogError(ex, "Failed to get top performer");
                return StatusCode(500, "Failed to retrieve top performer");
            }
        }

        [HttpGet("api/market-intelligence/summary")]
        public async Task<IActionResult> GetMarketIntelligenceSummary()
        {
            try
            {
                var summary = await _quiverService.GetMarketIntelligenceSummaryAsync();
                return Json(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get market intelligence summary");
                return StatusCode(500, "Failed to retrieve market intelligence data");
            }
        }

        [HttpGet("api/congressperson/{name}/portfolio")]
        public async Task<IActionResult> GetCongresspersonPortfolio(string name)
        {
            try
            {
                var portfolio = await _quiverService.GetCongresspersonPortfolioAsync(name);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio not found for {name}");
                }
                return Json(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get congressperson portfolio for {Name}", name);
                return StatusCode(500, "Failed to retrieve congressperson portfolio");
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