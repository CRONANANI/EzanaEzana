using EzanaEzana.Models;
using EzanaEzana.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuiverController : ControllerBase
    {
        private readonly IQuiverService _quiverService;
        private readonly ILogger<QuiverController> _logger;
        private readonly IConfiguration _configuration;

        public QuiverController(
            IQuiverService quiverService,
            ILogger<QuiverController> logger,
            IConfiguration configuration)
        {
            _quiverService = quiverService;
            _logger = logger;
            _configuration = configuration;
        }

        #region Market Intelligence Endpoints
        [HttpGet("market-intelligence/summary")]
        public async Task<ActionResult<Dictionary<string, MarketIntelligenceData>>> GetMarketIntelligenceSummary()
        {
            try
            {
                var summary = await _quiverService.GetMarketIntelligenceSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get market intelligence summary");
                return StatusCode(500, "Failed to retrieve market intelligence data");
            }
        }

        [HttpGet("congressional-trading")]
        public async Task<ActionResult<List<CongressionalTrading>>> GetCongressionalTrading([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetCongressionalTradingAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get congressional trading data");
                return StatusCode(500, "Failed to retrieve congressional trading data");
            }
        }

        [HttpGet("government-contracts")]
        public async Task<ActionResult<List<GovernmentContract>>> GetGovernmentContracts([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetGovernmentContractsAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get government contracts data");
                return StatusCode(500, "Failed to retrieve government contracts data");
            }
        }

        [HttpGet("house-trading")]
        public async Task<ActionResult<List<HouseTrading>>> GetHouseTrading([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetHouseTradingAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get house trading data");
                return StatusCode(500, "Failed to retrieve house trading data");
            }
        }

        [HttpGet("senator-trading")]
        public async Task<ActionResult<List<SenatorTrading>>> GetSenatorTrading([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetSenatorTradingAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get senator trading data");
                return StatusCode(500, "Failed to retrieve senator trading data");
            }
        }

        [HttpGet("lobbying-activity")]
        public async Task<ActionResult<List<LobbyingActivity>>> GetLobbyingActivity([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetLobbyingActivityAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get lobbying activity data");
                return StatusCode(500, "Failed to retrieve lobbying activity data");
            }
        }

        [HttpGet("patent-momentum")]
        public async Task<ActionResult<List<PatentMomentum>>> GetPatentMomentum([FromQuery] int limit = 100)
        {
            try
            {
                var data = await _quiverService.GetPatentMomentumAsync(limit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get patent momentum data");
                return StatusCode(500, "Failed to retrieve patent momentum data");
            }
        }
        #endregion

        #region Congressperson Portfolio Endpoints
        [HttpGet("congressperson/{name}/portfolio")]
        public async Task<ActionResult<CongresspersonPortfolio>> GetCongresspersonPortfolio(string name)
        {
            try
            {
                var portfolio = await _quiverService.GetCongresspersonPortfolioAsync(name);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio not found for {name}");
                }
                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get congressperson portfolio for {Name}", name);
                return StatusCode(500, "Failed to retrieve congressperson portfolio");
            }
        }
        #endregion

        #region Portfolio Analytics Endpoints
        [HttpGet("portfolio/summary")]
        public async Task<ActionResult<PortfolioSummary>> GetPortfolioSummary()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var summary = await _quiverService.GetPortfolioSummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get portfolio summary");
                return StatusCode(500, "Failed to retrieve portfolio summary");
            }
        }

        [HttpGet("portfolio/asset-allocation")]
        public async Task<ActionResult<AssetAllocation>> GetAssetAllocation([FromQuery] string breakdownType = "asset_class")
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var allocation = await _quiverService.GetAssetAllocationAsync(userId, breakdownType);
                return Ok(allocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get asset allocation");
                return StatusCode(500, "Failed to retrieve asset allocation");
            }
        }

        [HttpGet("portfolio/top-holdings")]
        public async Task<ActionResult<List<PortfolioHolding>>> GetTopHoldings([FromQuery] int count = 3)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var holdings = await _quiverService.GetTopHoldingsAsync(userId, count);
                return Ok(holdings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top holdings");
                return StatusCode(500, "Failed to retrieve top holdings");
            }
        }

        [HttpGet("portfolio/history")]
        public async Task<ActionResult<List<PortfolioDataPoint>>> GetPortfolioHistory([FromQuery] int months = 12)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var history = await _quiverService.GetPortfolioHistoryAsync(userId, months);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get portfolio history");
                return StatusCode(500, "Failed to retrieve portfolio history");
            }
        }
        #endregion

        #region Health Check Endpoint
        [HttpGet("health")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Health()
        {
            try
            {
                var isHealthy = await _quiverService.CheckApiHealthAsync();
                return Ok(new
                {
                    Service = "Quiver API Service",
                    Status = isHealthy ? "Healthy" : "Unhealthy",
                    ApiKey = !string.IsNullOrEmpty(_configuration["Quiver:ApiKey"]) ? "Configured" : "Not Configured",
                    BaseUrl = _configuration["Quiver:BaseUrl"] ?? "Not Configured",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new
                {
                    Service = "Quiver API Service",
                    Status = "Error",
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
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
