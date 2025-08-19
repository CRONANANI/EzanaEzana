using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class MarketResearchController : Controller
    {
        private readonly IMarketResearchService _marketResearchService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MarketResearchController> _logger;
        private readonly IConfiguration _configuration;

        public MarketResearchController(
            IMarketResearchService marketResearchService,
            UserManager<ApplicationUser> userManager,
            ILogger<MarketResearchController> logger,
            IConfiguration configuration)
        {
            _marketResearchService = marketResearchService;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        // GET: MarketResearch
        public async Task<IActionResult> Index()
        {
            try
            {
                var marketIntelligence = await _marketResearchService.GetMarketIntelligenceAsync();
                var economicOutlook = await _marketResearchService.GetEconomicOutlookAsync();

                var viewModel = new MarketResearchDashboardViewModel
                {
                    MarketIntelligence = marketIntelligence,
                    EconomicOutlook = economicOutlook
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading market research dashboard");
                return View("Error");
            }
        }

        // GET: MarketResearch/MarketAnalysis
        public async Task<IActionResult> MarketAnalysis()
        {
            try
            {
                var marketData = await _marketResearchService.GetMarketDataAsync();
                var marketIntelligence = await _marketResearchService.GetMarketIntelligenceAsync();

                var viewModel = new MarketAnalysisViewModel
                {
                    MarketData = marketData,
                    MarketIntelligence = marketIntelligence
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading market analysis");
                return View("Error");
            }
        }

        // GET: MarketResearch/CompanyResearch
        public async Task<IActionResult> CompanyResearch(string? symbol = null)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                {
                    return View(new CompanyResearchViewModel());
                }

                var companyProfile = await _marketResearchService.GetCompanyProfileAsync(symbol);
                var financials = await _marketResearchService.GetCompanyFinancialsAsync(symbol);
                var news = await _marketResearchService.GetCompanyNewsAsync(symbol);

                var viewModel = new CompanyResearchViewModel
                {
                    Symbol = symbol,
                    CompanyProfile = companyProfile,
                    Financials = financials,
                    News = news
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading company research for symbol {Symbol}", symbol);
                return View("Error");
            }
        }

        // GET: MarketResearch/EconomicIndicators
        public async Task<IActionResult> EconomicIndicators(string? category = null)
        {
            try
            {
                var indicators = await _marketResearchService.GetEconomicIndicatorsAsync(category);
                var economicOutlook = await _marketResearchService.GetEconomicOutlookAsync();

                var viewModel = new EconomicIndicatorsViewModel
                {
                    Indicators = indicators,
                    EconomicOutlook = economicOutlook,
                    SelectedCategory = category
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading economic indicators");
                return View("Error");
            }
        }

        // GET: MarketResearch/Stock/{symbol}
        public async Task<IActionResult> Stock(string symbol)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                {
                    return RedirectToAction(nameof(CompanyResearch));
                }

                var companyProfile = await _marketResearchService.GetCompanyProfileAsync(symbol);
                var marketData = await _marketResearchService.GetMarketDataBySymbolAsync(symbol);
                var financials = await _marketResearchService.GetCompanyFinancialsAsync(symbol);
                var news = await _marketResearchService.GetCompanyNewsAsync(symbol);

                var viewModel = new StockDetailViewModel
                {
                    Symbol = symbol,
                    CompanyProfile = companyProfile,
                    MarketData = marketData,
                    Financials = financials,
                    News = news
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading stock details for symbol {Symbol}", symbol);
                return View("Error");
            }
        }

        // GET: MarketResearch/TrendAnalysis/{symbol}
        public async Task<IActionResult> TrendAnalysis(string symbol, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                {
                    return RedirectToAction(nameof(CompanyResearch));
                }

                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;

                var trendAnalysis = await _marketResearchService.AnalyzeMarketTrendsAsync(symbol, start, end);

                var viewModel = new TrendAnalysisViewModel
                {
                    Symbol = symbol,
                    StartDate = start,
                    EndDate = end,
                    Analysis = trendAnalysis
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading trend analysis for symbol {Symbol}", symbol);
                return View("Error");
            }
        }

        // GET: MarketResearch/Compare
        public async Task<IActionResult> Compare(string? symbols = null)
        {
            try
            {
                if (string.IsNullOrEmpty(symbols))
                {
                    return View(new CompanyComparisonViewModel());
                }

                var symbolList = symbols.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToUpper())
                    .ToList();

                if (symbolList.Count < 2)
                {
                    TempData["Error"] = "Please provide at least 2 symbols to compare";
                    return View(new CompanyComparisonViewModel());
                }

                var comparison = await _marketResearchService.CompareCompaniesAsync(symbolList);

                var viewModel = new CompanyComparisonViewModel
                {
                    Companies = comparison.Companies,
                    ComparisonDate = comparison.ComparisonDate,
                    Symbols = symbols
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading company comparison");
                return View("Error");
            }
        }

        // POST: MarketResearch/Compare
        [HttpPost]
        public async Task<IActionResult> ComparePost([FromForm] string symbols)
        {
            try
            {
                if (string.IsNullOrEmpty(symbols))
                {
                    TempData["Error"] = "Please enter symbols to compare";
                    return RedirectToAction(nameof(Compare));
                }

                var symbolList = symbols.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim().ToUpper())
                    .ToList();

                if (symbolList.Count < 2)
                {
                    TempData["Error"] = "Please provide at least 2 symbols to compare";
                    return RedirectToAction(nameof(Compare));
                }

                var comparison = await _marketResearchService.CompareCompaniesAsync(symbolList);

                var viewModel = new CompanyComparisonViewModel
                {
                    Companies = comparison.Companies,
                    ComparisonDate = comparison.ComparisonDate,
                    Symbols = symbols
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing company comparison");
                TempData["Error"] = "An error occurred while processing the comparison";
                return RedirectToAction(nameof(Compare));
            }
        }

        // GET: MarketResearch/Search
        public async Task<IActionResult> Search(string? query = null, string? category = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return View(new MarketSearchViewModel());
                }

                var marketData = await _marketResearchService.GetMarketDataAsync(query);
                var indicators = await _marketResearchService.GetEconomicIndicatorsAsync(category);

                var viewModel = new MarketSearchViewModel
                {
                    Query = query,
                    Category = category,
                    MarketData = marketData,
                    EconomicIndicators = indicators
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing market search");
                return View("Error");
            }
        }

        // API Endpoints

        // GET: MarketResearch/API/MarketData
        [HttpGet("API/MarketData")]
        public async Task<IActionResult> GetMarketData([FromQuery] string? symbol = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var marketData = await _marketResearchService.GetMarketDataAsync(symbol, page, pageSize);
                return Ok(marketData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/CompanyProfile/{symbol}
        [HttpGet("API/CompanyProfile/{symbol}")]
        public async Task<IActionResult> GetCompanyProfile(string symbol)
        {
            try
            {
                var profile = await _marketResearchService.GetCompanyProfileAsync(symbol);
                if (profile == null)
                {
                    return NotFound($"Company profile not found for symbol {symbol}");
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company profile via API for symbol {Symbol}", symbol);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/TrendAnalysis/{symbol}
        [HttpGet("API/TrendAnalysis/{symbol}")]
        public async Task<IActionResult> GetTrendAnalysis(string symbol, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var analysis = await _marketResearchService.AnalyzeMarketTrendsAsync(symbol, startDate, endDate);
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trend analysis via API for symbol {Symbol}", symbol);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/MarketIntelligence
        [HttpGet("API/MarketIntelligence")]
        public async Task<IActionResult> GetMarketIntelligence()
        {
            try
            {
                var intelligence = await _marketResearchService.GetMarketIntelligenceAsync();
                return Ok(intelligence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market intelligence via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/EconomicOutlook
        [HttpGet("API/EconomicOutlook")]
        public async Task<IActionResult> GetEconomicOutlook()
        {
            try
            {
                var outlook = await _marketResearchService.GetEconomicOutlookAsync();
                return Ok(outlook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting economic outlook via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: MarketResearch/API/UpdateMarketData
        [HttpPost("API/UpdateMarketData")]
        public async Task<IActionResult> UpdateMarketData([FromBody] UpdateMarketDataRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _marketResearchService.UpdateMarketDataAsync(
                    request.Symbol,
                    request.CurrentPrice,
                    request.PriceChange,
                    request.PriceChangePercent,
                    request.Volume);

                if (success)
                {
                    return Ok(new { message = "Market data updated successfully" });
                }
                else
                {
                    return BadRequest("Failed to update market data");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating market data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: MarketResearch/API/LogActivity
        [HttpPost("API/LogActivity")]
        public async Task<IActionResult> LogActivity([FromBody] LogActivityRequest request)
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

                var success = await _marketResearchService.LogUserActivityAsync(
                    userId,
                    request.Category,
                    request.Action,
                    request.Details);

                if (success)
                {
                    return Ok(new { message = "Activity logged successfully" });
                }
                else
                {
                    return BadRequest("Failed to log activity");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging activity via API");
                return StatusCode(500, "Internal server error");
            }
        }

        #region Quiver API Endpoints

        // GET: MarketResearch/API/Quiver/CongressionalTrading
        [HttpGet("API/Quiver/CongressionalTrading")]
        public async Task<IActionResult> GetCongressionalTrading([FromQuery] int limit = 100)
        {
            try
            {
                var tradingData = await _marketResearchService.GetCongressionalTradingAsync(limit);
                return Ok(tradingData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting congressional trading data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/GovernmentContracts
        [HttpGet("API/Quiver/GovernmentContracts")]
        public async Task<IActionResult> GetGovernmentContracts([FromQuery] int limit = 100)
        {
            try
            {
                var contracts = await _marketResearchService.GetGovernmentContractsAsync(limit);
                return Ok(contracts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting government contracts via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/HouseTrading
        [HttpGet("API/Quiver/HouseTrading")]
        public async Task<IActionResult> GetHouseTrading([FromQuery] int limit = 100)
        {
            try
            {
                var houseTrading = await _marketResearchService.GetHouseTradingAsync(limit);
                return Ok(houseTrading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting House trading data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/SenatorTrading
        [HttpGet("API/Quiver/SenatorTrading")]
        public async Task<IActionResult> GetSenatorTrading([FromQuery] int limit = 100)
        {
            try
            {
                var senatorTrading = await _marketResearchService.GetSenatorTradingAsync(limit);
                return Ok(senatorTrading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Senator trading data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/LobbyingActivity
        [HttpGet("API/Quiver/LobbyingActivity")]
        public async Task<IActionResult> GetLobbyingActivity([FromQuery] int limit = 100)
        {
            try
            {
                var lobbying = await _marketResearchService.GetLobbyingActivityAsync(limit);
                return Ok(lobbying);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lobbying activity via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/PatentMomentum
        [HttpGet("API/Quiver/PatentMomentum")]
        public async Task<IActionResult> GetPatentMomentum([FromQuery] int limit = 100)
        {
            try
            {
                var patents = await _marketResearchService.GetPatentMomentumAsync(limit);
                return Ok(patents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patent momentum data via API");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: MarketResearch/API/Quiver/MarketIntelligenceSummary
        [HttpGet("API/Quiver/MarketIntelligenceSummary")]
        public async Task<IActionResult> GetMarketIntelligenceSummary()
        {
            try
            {
                var summary = await _marketResearchService.GetMarketIntelligenceSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market intelligence summary via API");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        // GET: MarketResearch/TestQuiver
        [AllowAnonymous]
        public async Task<IActionResult> TestQuiver()
        {
            try
            {
                var viewModel = new QuiverTestViewModel
                {
                    ApiKeyConfigured = !string.IsNullOrEmpty(_configuration["Quiver:ApiKey"]),
                    BaseUrl = _configuration["Quiver:BaseUrl"] ?? "Not configured",
                    TimeoutSeconds = int.Parse(_configuration["Quiver:TimeoutSeconds"] ?? "30")
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Quiver test page");
                return View("Error");
            }
        }
    }

    // Request/Response Models for API endpoints
    public class UpdateMarketDataRequest
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PriceChange { get; set; }
        public decimal PriceChangePercent { get; set; }
        public long Volume { get; set; }
    }

    public class LogActivityRequest
    {
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    // View Models for MVC views
    public class MarketResearchDashboardViewModel
    {
        public MarketIntelligenceViewModel MarketIntelligence { get; set; } = new();
        public EconomicOutlookViewModel EconomicOutlook { get; set; } = new();
    }

    public class MarketAnalysisViewModel
    {
        public IEnumerable<MarketData> MarketData { get; set; } = new List<MarketData>();
        public MarketIntelligenceViewModel MarketIntelligence { get; set; } = new();
    }

    public class CompanyResearchViewModel
    {
        public string? Symbol { get; set; }
        public CompanyProfile? CompanyProfile { get; set; }
        public IEnumerable<CompanyFinancial> Financials { get; set; } = new List<CompanyFinancial>();
        public IEnumerable<CompanyNews> News { get; set; } = new List<CompanyNews>();
    }

    public class EconomicIndicatorsViewModel
    {
        public IEnumerable<EconomicIndicator> Indicators { get; set; } = new List<EconomicIndicator>();
        public EconomicOutlookViewModel EconomicOutlook { get; set; } = new();
        public string? SelectedCategory { get; set; }
    }

    public class StockDetailViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public CompanyProfile? CompanyProfile { get; set; }
        public MarketData? MarketData { get; set; }
        public IEnumerable<CompanyFinancial> Financials { get; set; } = new List<CompanyFinancial>();
        public IEnumerable<CompanyNews> News { get; set; } = new List<CompanyNews>();
    }

    public class TrendAnalysisViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MarketTrendAnalysisViewModel Analysis { get; set; } = new();
    }

    public class MarketSearchViewModel
    {
        public string? Query { get; set; }
        public string? Category { get; set; }
        public IEnumerable<MarketData> MarketData { get; set; } = new List<MarketData>();
        public IEnumerable<EconomicIndicator> EconomicIndicators { get; set; } = new List<EconomicIndicator>();
    }

    public class QuiverTestViewModel
    {
        public bool ApiKeyConfigured { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; }
    }
}
