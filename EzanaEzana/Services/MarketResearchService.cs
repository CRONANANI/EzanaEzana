using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using EzanaEzana.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Services
{
    public class MarketResearchService : IMarketResearchService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MarketResearchService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuiverService _quiverService;

        public MarketResearchService(
            ApplicationDbContext context, 
            ILogger<MarketResearchService> logger,
            UserManager<ApplicationUser> userManager,
            IQuiverService quiverService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _quiverService = quiverService;
        }

        public async Task<IEnumerable<MarketData>> GetMarketDataAsync(string? symbol = null, int page = 1, int pageSize = 50)
        {
            try
            {
                var query = _context.MarketData
                    .Include(md => md.Company)
                    .Where(md => md.IsActive);

                if (!string.IsNullOrEmpty(symbol))
                {
                    query = query.Where(md => md.Symbol.Contains(symbol.ToUpper()));
                }

                return await query
                    .OrderByDescending(md => md.LastUpdated)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market data");
                return new List<MarketData>();
            }
        }

        public async Task<MarketData?> GetMarketDataBySymbolAsync(string symbol)
        {
            try
            {
                return await _context.MarketData
                    .Include(md => md.Company)
                    .FirstOrDefaultAsync(md => md.Symbol == symbol.ToUpper() && md.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market data for symbol {Symbol}", symbol);
                return null;
            }
        }

        public async Task<IEnumerable<MarketDataHistory>> GetMarketDataHistoryAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.MarketDataHistory
                    .Where(mdh => mdh.Symbol == symbol.ToUpper() && 
                                 mdh.Date >= startDate && 
                                 mdh.Date <= endDate)
                    .OrderBy(mdh => mdh.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market data history for symbol {Symbol}", symbol);
                return new List<MarketDataHistory>();
            }
        }

        public async Task<CompanyProfile?> GetCompanyProfileAsync(string symbol)
        {
            try
            {
                return await _context.CompanyProfiles
                    .Include(cp => cp.MarketData)
                    .Include(cp => cp.Financials)
                    .FirstOrDefaultAsync(cp => cp.Symbol == symbol.ToUpper() && cp.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company profile for symbol {Symbol}", symbol);
                return null;
            }
        }

        public async Task<IEnumerable<CompanyFinancial>> GetCompanyFinancialsAsync(string symbol, int page = 1, int pageSize = 20)
        {
            try
            {
                return await _context.CompanyFinancials
                    .Where(cf => cf.Symbol == symbol.ToUpper())
                    .OrderByDescending(cf => cf.ReportDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company financials for symbol {Symbol}", symbol);
                return new List<CompanyFinancial>();
            }
        }

        public async Task<IEnumerable<CompanyNews>> GetCompanyNewsAsync(string symbol, int page = 1, int pageSize = 20)
        {
            try
            {
                return await _context.CompanyNews
                    .Where(cn => cn.Symbol == symbol.ToUpper() && cn.IsActive)
                    .OrderByDescending(cn => cn.PublishedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company news for symbol {Symbol}", symbol);
                return new List<CompanyNews>();
            }
        }

        public async Task<IEnumerable<EconomicIndicator>> GetEconomicIndicatorsAsync(string? category = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.EconomicIndicators
                    .Where(ei => ei.IsActive);

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(ei => ei.Category == category);
                }

                return await query
                    .OrderBy(ei => ei.Category)
                    .ThenBy(ei => ei.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting economic indicators");
                return new List<EconomicIndicator>();
            }
        }

        public async Task<IEnumerable<EconomicIndicatorHistory>> GetEconomicIndicatorHistoryAsync(int indicatorId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.EconomicIndicatorHistory
                    .Where(eih => eih.IndicatorId == indicatorId && 
                                 eih.Date >= startDate && 
                                 eih.Date <= endDate)
                    .OrderBy(eih => eih.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting economic indicator history for indicator {IndicatorId}", indicatorId);
                return new List<EconomicIndicatorHistory>();
            }
        }

        public async Task<MarketTrendAnalysisViewModel> AnalyzeMarketTrendsAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                var history = await _context.MarketDataHistory
                    .Where(mdh => mdh.Symbol == symbol.ToUpper() && 
                                 mdh.Date >= startDate && 
                                 mdh.Date <= endDate)
                    .OrderBy(mdh => mdh.Date)
                    .ToListAsync();

                if (!history.Any())
                {
                    return new MarketTrendAnalysisViewModel();
                }

                var prices = history.Select(h => h.ClosePrice).ToList();
                var dates = history.Select(h => h.Date).ToList();

                // Calculate simple moving averages
                var sma20 = CalculateSMA(prices, 20);
                var sma50 = CalculateSMA(prices, 50);

                // Calculate volatility
                var volatility = CalculateVolatility(prices);

                // Calculate trend direction
                var trendDirection = CalculateTrendDirection(prices);

                // Calculate support and resistance levels
                var supportLevel = prices.Min();
                var resistanceLevel = prices.Max();

                return new MarketTrendAnalysisViewModel
                {
                    Symbol = symbol,
                    StartDate = startDate,
                    EndDate = endDate,
                    DataPoints = history.Count,
                    CurrentPrice = prices.Last(),
                    PriceChange = prices.Last() - prices.First(),
                    PriceChangePercent = prices.First() > 0 ? ((prices.Last() - prices.First()) / prices.First()) * 100 : 0,
                    Volatility = volatility,
                    TrendDirection = trendDirection,
                    SupportLevel = supportLevel,
                    ResistanceLevel = resistanceLevel,
                    SMA20 = sma20,
                    SMA50 = sma50,
                    PriceHistory = history.Select(h => new PriceDataPoint
                    {
                        Date = h.Date,
                        Price = h.ClosePrice,
                        Volume = h.Volume
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing market trends for symbol {Symbol}", symbol);
                return new MarketTrendAnalysisViewModel();
            }
        }

        public async Task<CompanyComparisonViewModel> CompareCompaniesAsync(List<string> symbols)
        {
            try
            {
                var companies = new List<CompanyComparisonItemViewModel>();

                foreach (var symbol in symbols)
                {
                    var profile = await _context.CompanyProfiles
                        .Include(cp => cp.MarketData)
                        .FirstOrDefaultAsync(cp => cp.Symbol == symbol.ToUpper());

                    if (profile != null)
                    {
                        var marketData = profile.MarketData?.FirstOrDefault();
                        
                        companies.Add(new CompanyComparisonItemViewModel
                        {
                            Symbol = profile.Symbol,
                            CompanyName = profile.CompanyName,
                            Sector = profile.Sector,
                            Industry = profile.Industry,
                            MarketCap = profile.MarketCap,
                            CurrentPrice = marketData?.CurrentPrice ?? 0,
                            PriceChange = marketData?.PriceChange ?? 0,
                            PriceChangePercent = marketData?.PriceChangePercent ?? 0,
                            Volume = marketData?.Volume ?? 0,
                            PE_Ratio = profile.PE_Ratio,
                            DividendYield = profile.DividendYield,
                            Beta = profile.Beta
                        });
                    }
                }

                return new CompanyComparisonViewModel
                {
                    Companies = companies,
                    ComparisonDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comparing companies: {Symbols}", string.Join(", ", symbols));
                return new CompanyComparisonViewModel();
            }
        }

        public async Task<MarketIntelligenceViewModel> GetMarketIntelligenceAsync()
        {
            try
            {
                var topGainers = await _context.MarketData
                    .Where(md => md.IsActive && md.PriceChangePercent > 0)
                    .OrderByDescending(md => md.PriceChangePercent)
                    .Take(10)
                    .Select(md => new TopMoverViewModel
                    {
                        Symbol = md.Symbol,
                        CompanyName = md.Company?.CompanyName ?? md.Symbol,
                        CurrentPrice = md.CurrentPrice,
                        PriceChange = md.PriceChange,
                        PriceChangePercent = md.PriceChangePercent,
                        Volume = md.Volume
                    })
                    .ToListAsync();

                var topLosers = await _context.MarketData
                    .Where(md => md.IsActive && md.PriceChangePercent < 0)
                    .OrderBy(md => md.PriceChangePercent)
                    .Take(10)
                    .Select(md => new TopMoverViewModel
                    {
                        Symbol = md.Symbol,
                        CompanyName = md.Company?.CompanyName ?? md.Symbol,
                        CurrentPrice = md.CurrentPrice,
                        PriceChange = md.PriceChange,
                        PriceChangePercent = md.PriceChangePercent,
                        Volume = md.Volume
                    })
                    .ToListAsync();

                var mostActive = await _context.MarketData
                    .Where(md => md.IsActive)
                    .OrderByDescending(md => md.Volume)
                    .Take(10)
                    .Select(md => new TopMoverViewModel
                    {
                        Symbol = md.Symbol,
                        CompanyName = md.Company?.CompanyName ?? md.Symbol,
                        CurrentPrice = md.CurrentPrice,
                        PriceChange = md.PriceChange,
                        PriceChangePercent = md.PriceChangePercent,
                        Volume = md.Volume
                    })
                    .ToListAsync();

                return new MarketIntelligenceViewModel
                {
                    TopGainers = topGainers,
                    TopLosers = topLosers,
                    MostActive = mostActive,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market intelligence");
                return new MarketIntelligenceViewModel();
            }
        }

        public async Task<EconomicOutlookViewModel> GetEconomicOutlookAsync()
        {
            try
            {
                var indicators = await _context.EconomicIndicators
                    .Where(ei => ei.IsActive)
                    .ToListAsync();

                var outlook = new EconomicOutlookViewModel
                {
                    Indicators = new List<EconomicIndicatorOutlookViewModel>(),
                    LastUpdated = DateTime.UtcNow
                };

                foreach (var indicator in indicators)
                {
                    var recentHistory = await _context.EconomicIndicatorHistory
                        .Where(eih => eih.IndicatorId == indicator.Id)
                        .OrderByDescending(eih => eih.Date)
                        .Take(2)
                        .ToListAsync();

                    if (recentHistory.Count >= 2)
                    {
                        var currentValue = recentHistory[0].Value;
                        var previousValue = recentHistory[1].Value;
                        var change = currentValue - previousValue;
                        var changePercent = previousValue != 0 ? (change / previousValue) * 100 : 0;

                        var trend = change > 0 ? "Improving" : change < 0 ? "Declining" : "Stable";

                        outlook.Indicators.Add(new EconomicIndicatorOutlookViewModel
                        {
                            IndicatorId = indicator.Id,
                            Name = indicator.Name,
                            Category = indicator.Category,
                            CurrentValue = currentValue,
                            PreviousValue = previousValue,
                            Change = change,
                            ChangePercent = changePercent,
                            Trend = trend,
                            LastUpdated = recentHistory[0].Date
                        });
                    }
                }

                return outlook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting economic outlook");
                return new EconomicOutlookViewModel();
            }
        }

        public async Task<bool> UpdateMarketDataAsync(string symbol, decimal currentPrice, decimal priceChange, decimal priceChangePercent, long volume)
        {
            try
            {
                var marketData = await _context.MarketData
                    .FirstOrDefaultAsync(md => md.Symbol == symbol.ToUpper());

                if (marketData == null)
                {
                    // Create new market data entry
                    marketData = new MarketData
                    {
                        Symbol = symbol.ToUpper(),
                        CurrentPrice = currentPrice,
                        PriceChange = priceChange,
                        PriceChangePercent = priceChangePercent,
                        Volume = volume,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow
                    };
                    _context.MarketData.Add(marketData);
                }
                else
                {
                    // Update existing market data
                    marketData.CurrentPrice = currentPrice;
                    marketData.PriceChange = priceChange;
                    marketData.PriceChangePercent = priceChangePercent;
                    marketData.Volume = volume;
                    marketData.LastUpdated = DateTime.UtcNow;
                }

                // Add to history
                var historyEntry = new MarketDataHistory
                {
                    Symbol = symbol.ToUpper(),
                    Date = DateTime.UtcNow.Date,
                    OpenPrice = marketData.OpenPrice,
                    HighPrice = Math.Max(marketData.HighPrice, currentPrice),
                    LowPrice = Math.Min(marketData.LowPrice, currentPrice),
                    ClosePrice = currentPrice,
                    Volume = volume,
                    CreatedAt = DateTime.UtcNow
                };

                _context.MarketDataHistory.Add(historyEntry);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Market data updated for symbol {Symbol}", symbol);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating market data for symbol {Symbol}", symbol);
                return false;
            }
        }

        public async Task<bool> LogUserActivityAsync(string userId, string category, string action, string details)
        {
            try
            {
                var activity = new UserActivity
                {
                    UserId = userId,
                    Category = category,
                    Action = action,
                    Details = details,
                    Timestamp = DateTime.UtcNow
                };

                _context.UserActivities.Add(activity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user activity for user {UserId}", userId);
                return false;
            }
        }

        private double CalculateSMA(List<decimal> prices, int period)
        {
            if (prices.Count < period)
                return 0;

            var recentPrices = prices.TakeLast(period);
            return (double)recentPrices.Average();
        }

        private double CalculateVolatility(List<decimal> prices)
        {
            if (prices.Count < 2)
                return 0;

            var returns = new List<double>();
            for (int i = 1; i < prices.Count; i++)
            {
                var returnValue = (double)((prices[i] - prices[i - 1]) / prices[i - 1]);
                returns.Add(returnValue);
            }

            var mean = returns.Average();
            var variance = returns.Select(r => Math.Pow(r - mean, 2)).Average();
            return Math.Sqrt(variance);
        }

        private string CalculateTrendDirection(List<decimal> prices)
        {
            if (prices.Count < 2)
                return "Unknown";

            var firstPrice = prices.First();
            var lastPrice = prices.Last();
            var change = lastPrice - firstPrice;
            var changePercent = firstPrice > 0 ? (change / firstPrice) * 100 : 0;

            if (changePercent > 5)
                return "Strongly Bullish";
            else if (changePercent > 1)
                return "Bullish";
            else if (changePercent < -5)
                return "Strongly Bearish";
            else if (changePercent < -1)
                return "Bearish";
            else
                return "Sideways";
        }

        #region Quiver API Integration Methods

        public async Task<IEnumerable<CongressionalTrading>> GetCongressionalTradingAsync(int limit = 100)
        {
            try
            {
                var tradingData = await _quiverService.GetCongressionalTradingAsync(limit);
                if (tradingData != null && tradingData.Any())
                {
                    return tradingData;
                }
                
                _logger.LogWarning("No congressional trading data returned from Quiver API, using database fallback");
                return await GetCongressionalTradingFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting congressional trading data from Quiver API, using database fallback");
                return await GetCongressionalTradingFromDatabaseAsync(limit);
            }
        }

        public async Task<IEnumerable<GovernmentContract>> GetGovernmentContractsAsync(int limit = 100)
        {
            try
            {
                var contracts = await _quiverService.GetGovernmentContractsAsync(limit);
                if (contracts != null && contracts.Any())
                {
                    return contracts;
                }
                
                _logger.LogWarning("No government contracts returned from Quiver API, using database fallback");
                return await GetGovernmentContractsFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting government contracts from Quiver API, using database fallback");
                return await GetGovernmentContractsFromDatabaseAsync(limit);
            }
        }

        public async Task<IEnumerable<HouseTrading>> GetHouseTradingAsync(int limit = 100)
        {
            try
            {
                var houseTrading = await _quiverService.GetHouseTradingAsync(limit);
                if (houseTrading != null && houseTrading.Any())
                {
                    return houseTrading;
                }
                
                _logger.LogWarning("No House trading data returned from Quiver API, using database fallback");
                return await GetHouseTradingFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting House trading data from Quiver API, using database fallback");
                return await GetHouseTradingFromDatabaseAsync(limit);
            }
        }

        public async Task<IEnumerable<SenatorTrading>> GetSenatorTradingAsync(int limit = 100)
        {
            try
            {
                var senatorTrading = await _quiverService.GetSenatorTradingAsync(limit);
                if (senatorTrading != null && senatorTrading.Any())
                {
                    return senatorTrading;
                }
                
                _logger.LogWarning("No Senator trading data returned from Quiver API, using database fallback");
                return await GetSenatorTradingFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Senator trading data from Quiver API, using database fallback");
                return await GetSenatorTradingFromDatabaseAsync(limit);
            }
        }

        public async Task<IEnumerable<LobbyingActivity>> GetLobbyingActivityAsync(int limit = 100)
        {
            try
            {
                var lobbying = await _quiverService.GetLobbyingActivityAsync(limit);
                if (lobbying != null && lobbying.Any())
                {
                    return lobbying;
                }
                
                _logger.LogWarning("No lobbying activity data returned from Quiver API, using database fallback");
                return await GetLobbyingActivityFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lobbying activity from Quiver API, using database fallback");
                return await GetLobbyingActivityFromDatabaseAsync(limit);
            }
        }

        public async Task<IEnumerable<PatentMomentum>> GetPatentMomentumAsync(int limit = 100)
        {
            try
            {
                var patents = await _quiverService.GetPatentMomentumAsync(limit);
                if (patents != null && patents.Any())
                {
                    return patents;
                }
                
                _logger.LogWarning("No patent momentum data returned from Quiver API, using database fallback");
                return await GetPatentMomentumFromDatabaseAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patent momentum from Quiver API, using database fallback");
                return await GetPatentMomentumFromDatabaseAsync(limit);
            }
        }

        public async Task<Dictionary<string, object>> GetMarketIntelligenceSummaryAsync()
        {
            try
            {
                var summary = new Dictionary<string, object>();

                // Get congressional trading summary
                var congressionalTrading = await GetCongressionalTradingAsync(100);
                summary["congressionalTrading"] = new
                {
                    totalTrades = congressionalTrading.Count(),
                    totalVolume = congressionalTrading.Sum(t => t.Amount),
                    activeTraders = congressionalTrading.Select(t => t.CongressPersonName).Distinct().Count(),
                    lastUpdated = DateTime.UtcNow
                };

                // Get government contracts summary
                var governmentContracts = await GetGovernmentContractsAsync(100);
                summary["governmentContracts"] = new
                {
                    totalContracts = governmentContracts.Count(),
                    totalValue = governmentContracts.Sum(c => c.ContractValue),
                    activeCompanies = governmentContracts.Select(c => c.CompanyName).Distinct().Count(),
                    lastUpdated = DateTime.UtcNow
                };

                // Get House trading summary
                var houseTrading = await GetHouseTradingAsync(100);
                summary["houseTrading"] = new
                {
                    totalTrades = houseTrading.Count(),
                    totalVolume = houseTrading.Sum(t => t.Amount),
                    activeTraders = houseTrading.Select(t => t.CongressPersonName).Distinct().Count(),
                    lastUpdated = DateTime.UtcNow
                };

                // Get Senator trading summary
                var senatorTrading = await GetSenatorTradingAsync(100);
                summary["senatorTrading"] = new
                {
                    totalTrades = senatorTrading.Count(),
                    totalVolume = senatorTrading.Sum(t => t.Amount),
                    activeTraders = senatorTrading.Select(t => t.CongressPersonName).Distinct().Count(),
                    lastUpdated = DateTime.UtcNow
                };

                // Get lobbying activity summary
                var lobbying = await GetLobbyingActivityAsync(100);
                summary["lobbyingActivity"] = new
                {
                    totalReports = lobbying.Count(),
                    totalSpending = lobbying.Sum(l => l.Amount),
                    activeFirms = lobbying.Select(l => l.FirmName).Distinct().Count(),
                    lastUpdated = DateTime.UtcNow
                };

                // Get patent momentum summary
                var patents = await GetPatentMomentumAsync(100);
                summary["patentMomentum"] = new
                {
                    totalPatents = patents.Count(),
                    activePatents = patents.Count(p => p.Status == "Active"),
                    pendingPatents = patents.Count(p => p.Status == "Pending"),
                    lastUpdated = DateTime.UtcNow
                };

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market intelligence summary");
                return new Dictionary<string, object>();
            }
        }

        #endregion

        #region Database Fallback Methods

        private async Task<IEnumerable<CongressionalTrading>> GetCongressionalTradingFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<CongressionalTrading>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting congressional trading from database");
                return new List<CongressionalTrading>();
            }
        }

        private async Task<IEnumerable<GovernmentContract>> GetGovernmentContractsFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<GovernmentContract>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting government contracts from database");
                return new List<GovernmentContract>();
            }
        }

        private async Task<IEnumerable<HouseTrading>> GetHouseTradingFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<HouseTrading>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting House trading from database");
                return new List<HouseTrading>();
            }
        }

        private async Task<IEnumerable<SenatorTrading>> GetSenatorTradingFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<SenatorTrading>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Senator trading from database");
                return new List<SenatorTrading>();
            }
        }

        private async Task<IEnumerable<LobbyingActivity>> GetLobbyingActivityFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<LobbyingActivity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lobbying activity from database");
                return new List<LobbyingActivity>();
            }
        }

        private async Task<IEnumerable<PatentMomentum>> GetPatentMomentumFromDatabaseAsync(int limit)
        {
            try
            {
                // This would query the local database if Quiver API fails
                // For now, return empty list as we don't have this data in our models yet
                return new List<PatentMomentum>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patent momentum from database");
                return new List<PatentMomentum>();
            }
        }

        #endregion
    }
}
