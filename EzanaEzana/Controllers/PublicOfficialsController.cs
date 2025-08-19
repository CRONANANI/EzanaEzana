using EzanaEzana.Models;
using EzanaEzana.Models.PublicOfficials;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EzanaEzana.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicOfficialsController : ControllerBase
    {
        private readonly IPublicOfficialsService _publicOfficialsService;
        private readonly ILogger<PublicOfficialsController> _logger;

        public PublicOfficialsController(
            IPublicOfficialsService publicOfficialsService,
            ILogger<PublicOfficialsController> logger)
        {
            _publicOfficialsService = publicOfficialsService;
            _logger = logger;
        }

        #region Individual Card Endpoints
        [HttpGet("historical-congress-trading")]
        public async Task<ActionResult<HistoricalCongressTradingCard>> GetHistoricalCongressTrading(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetHistoricalCongressTradingCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving historical congress trading data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("government-contracts")]
        public async Task<ActionResult<GovernmentContractsCard>> GetGovernmentContracts(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetGovernmentContractsCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving government contracts data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("house-trading")]
        public async Task<ActionResult<HouseTradingCard>> GetHouseTrading(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetHouseTradingCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving house trading data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("lobbying-activity")]
        public async Task<ActionResult<LobbyingActivityCard>> GetLobbyingActivity(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetLobbyingActivityCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lobbying activity data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("senator-trading")]
        public async Task<ActionResult<SenatorTradingCard>> GetSenatorTrading(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetSenatorTradingCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving senator trading data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("patent-momentum")]
        public async Task<ActionResult<PatentMomentumCard>> GetPatentMomentum(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetPatentMomentumCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patent momentum data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("market-sentiment")]
        public async Task<ActionResult<MarketSentimentCard>> GetMarketSentiment(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var card = await _publicOfficialsService.GetMarketSentimentCardAsync(useMockData);
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving market sentiment data");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region Summary and Utility Endpoints
        [HttpGet("summary")]
        public async Task<ActionResult<PublicOfficialsSummary>> GetPublicOfficialsSummary(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var summary = await _publicOfficialsService.GetPublicOfficialsSummaryAsync(useMockData);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public officials summary");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all-cards")]
        public async Task<ActionResult<Dictionary<string, object>>> GetAllCards(
            [FromQuery] bool useMockData = false)
        {
            try
            {
                var allCards = new Dictionary<string, object>
                {
                    ["HistoricalCongressTrading"] = await _publicOfficialsService.GetHistoricalCongressTradingCardAsync(useMockData),
                    ["GovernmentContracts"] = await _publicOfficialsService.GetGovernmentContractsCardAsync(useMockData),
                    ["HouseTrading"] = await _publicOfficialsService.GetHouseTradingCardAsync(useMockData),
                    ["LobbyingActivity"] = await _publicOfficialsService.GetLobbyingActivityCardAsync(useMockData),
                    ["SenatorTrading"] = await _publicOfficialsService.GetSenatorTradingCardAsync(useMockData),
                    ["PatentMomentum"] = await _publicOfficialsService.GetPatentMomentumCardAsync(useMockData),
                    ["MarketSentiment"] = await _publicOfficialsService.GetMarketSentimentCardAsync(useMockData)
                };

                return Ok(allCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all public officials cards");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("mock-data")]
        public async Task<ActionResult<Dictionary<string, object>>> GetMockData()
        {
            try
            {
                var mockData = await _publicOfficialsService.GetMockPublicOfficialsData();
                return Ok(mockData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mock public officials data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<bool>> RefreshFromQuiver()
        {
            try
            {
                var success = await _publicOfficialsService.RefreshFromQuiverAsync();
                if (success)
                {
                    return Ok(new { success = true, message = "Data refreshed successfully from Quiver API" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to refresh data from Quiver API" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing data from Quiver API");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("health")]
        public async Task<ActionResult<bool>> CheckQuiverApiHealth()
        {
            try
            {
                var isHealthy = await _publicOfficialsService.CheckQuiverApiHealthAsync();
                return Ok(new { healthy = isHealthy, timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Quiver API health");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("api-access")]
        public async Task<ActionResult<object>> GetApiAccessInfo()
        {
            try
            {
                var hasAccess = await _publicOfficialsService.HasQuiverApiAccessAsync();
                var apiKey = await _publicOfficialsService.GetQuiverApiKeyAsync();
                
                return Ok(new
                {
                    hasApiAccess = hasAccess,
                    hasApiKey = !string.IsNullOrEmpty(apiKey),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking API access info");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
    }
}
