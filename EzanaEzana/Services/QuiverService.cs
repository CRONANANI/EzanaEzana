using EzanaEzana.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EzanaEzana.Services
{
    public class QuiverService : IQuiverService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<QuiverService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly int _timeoutSeconds;

        public QuiverService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<QuiverService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _apiKey = _configuration["Quiver:ApiKey"];
            _baseUrl = _configuration["Quiver:BaseUrl"];
            _timeoutSeconds = int.Parse(_configuration["Quiver:TimeoutSeconds"] ?? "30");
            
            _httpClient.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
        }

        #region Congressional Trading
        public async Task<List<CongressionalTrading>> GetCongressionalTradingAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/congressionaltrading";
                var response = await MakeQuiverApiCallAsync<List<CongressionalTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch congressional trading data from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockCongressionalTradingData(limit);
        }

        public async Task<CongresspersonPortfolio> GetCongresspersonPortfolioAsync(string congresspersonName)
        {
            try
            {
                var endpoint = $"{_baseUrl}/congressperson/{Uri.EscapeDataString(congresspersonName)}/portfolio";
                var response = await MakeQuiverApiCallAsync<CongresspersonPortfolio>(endpoint);
                
                if (response != null)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch congressperson portfolio from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockCongresspersonPortfolio(congresspersonName);
        }
        #endregion

        #region Government Contracts
        public async Task<List<GovernmentContract>> GetGovernmentContractsAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/governmentcontracts";
                var response = await MakeQuiverApiCallAsync<List<GovernmentContract>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch government contracts from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockGovernmentContractsData(limit);
        }
        #endregion

        #region House Trading
        public async Task<List<HouseTrading>> GetHouseTradingAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/housetrading";
                var response = await MakeQuiverApiCallAsync<List<HouseTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch house trading from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockHouseTradingData(limit);
        }
        #endregion

        #region Senator Trading
        public async Task<List<SenatorTrading>> GetSenatorTradingAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/senatortrading";
                var response = await MakeQuiverApiCallAsync<List<SenatorTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch senator trading from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockSenatorTradingData(limit);
        }
        #endregion

        #region Lobbying Activity
        public async Task<List<LobbyingActivity>> GetLobbyingActivityAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/lobbying";
                var response = await MakeQuiverApiCallAsync<List<LobbyingActivity>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch lobbying activity from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockLobbyingData(limit);
        }
        #endregion

        #region Patent Momentum
        public async Task<List<PatentMomentum>> GetPatentMomentumAsync(int limit = 100)
        {
            try
            {
                var endpoint = $"{_baseUrl}/patentmomentum";
                var response = await MakeQuiverApiCallAsync<List<PatentMomentum>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return response.Take(limit).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch patent momentum from Quiver API, using mock data");
            }

            // Fallback to mock data
            return GenerateMockPatentMomentumData(limit);
        }
        #endregion

        #region Market Intelligence Summary
        public async Task<Dictionary<string, MarketIntelligenceData>> GetMarketIntelligenceSummaryAsync()
        {
            var summary = new Dictionary<string, MarketIntelligenceData>();

            try
            {
                // Fetch all data types concurrently
                var congressionalTrading = await GetCongressionalTradingAsync(25);
                var governmentContracts = await GetGovernmentContractsAsync(25);
                var houseTrading = await GetHouseTradingAsync(25);
                var senatorTrading = await GetSenatorTradingAsync(25);
                var lobbyingActivity = await GetLobbyingActivityAsync(25);
                var patentMomentum = await GetPatentMomentumAsync(25);

                summary["congress-trading"] = new MarketIntelligenceData
                {
                    Title = "Historical Congress Trading",
                    Subtitle = "Comprehensive trading history by members of Congress",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Congressperson", "Party", "Chamber", "Trade Type", "Amount", "Owner" },
                    Data = congressionalTrading.Cast<object>().ToList(),
                    TotalCount = congressionalTrading.Count
                };

                summary["government-contracts"] = new MarketIntelligenceData
                {
                    Title = "Historical Government Contracts",
                    Subtitle = "Government contract awards and spending data",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Contract Value", "Agency", "Contract Type", "Description" },
                    Data = governmentContracts.Cast<object>().ToList(),
                    TotalCount = governmentContracts.Count
                };

                summary["house-trading"] = new MarketIntelligenceData
                {
                    Title = "Historical House Trading",
                    Subtitle = "Trading activity by House of Representatives members",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Representative", "Party", "State", "Trade Type", "Amount" },
                    Data = houseTrading.Cast<object>().ToList(),
                    TotalCount = houseTrading.Count
                };

                summary["senator-trading"] = new MarketIntelligenceData
                {
                    Title = "Historical Senator Trading",
                    Subtitle = "Trading activity by Senate members",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Senator", "Party", "State", "Trade Type", "Amount" },
                    Data = senatorTrading.Cast<object>().ToList(),
                    TotalCount = senatorTrading.Count
                };

                summary["lobbying"] = new MarketIntelligenceData
                {
                    Title = "Historical Lobbying Activity",
                    Subtitle = "Lobbying expenditures and activities by companies",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Lobbying Firm", "Amount", "Issues", "Registrant" },
                    Data = lobbyingActivity.Cast<object>().ToList(),
                    TotalCount = lobbyingActivity.Count
                };

                summary["patent-momentum"] = new MarketIntelligenceData
                {
                    Title = "Patent Momentum",
                    Subtitle = "Patent activity and innovation metrics by company",
                    Columns = new List<string> { "Date", "Ticker", "Company", "Patent Count", "Patent Type", "Innovation Score", "Industry" },
                    Data = patentMomentum.Cast<object>().ToList(),
                    TotalCount = patentMomentum.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate market intelligence summary");
            }

            return summary;
        }
        #endregion

        #region Portfolio Analytics
        public async Task<PortfolioSummary> GetPortfolioSummaryAsync(string userId)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockPortfolioSummary();
        }

        public async Task<AssetAllocation> GetAssetAllocationAsync(string userId, string breakdownType = "asset_class")
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockAssetAllocation(breakdownType);
        }

        public async Task<List<PortfolioHolding>> GetTopHoldingsAsync(string userId, int count = 3)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockTopHoldings(count);
        }

        public async Task<List<PortfolioDataPoint>> GetPortfolioHistoryAsync(string userId, int months = 12)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockPortfolioHistory(months);
        }

        public async Task<RiskScore> GetRiskScoreAsync(string userId)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockRiskScore();
        }

        public async Task<DividendIncome> GetDividendIncomeAsync(string userId)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockDividendIncome();
        }

        public async Task<TopPerformer> GetTopPerformerAsync(string userId)
        {
            // This would typically integrate with your portfolio service
            // For now, returning mock data
            return GenerateMockTopPerformer();
        }
        #endregion

        #region Health Check
        public async Task<bool> CheckApiHealthAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey) || _apiKey == "your-quiver-api-key")
                {
                    return false;
                }

                // Try to make a simple API call to test connectivity
                var endpoint = $"{_baseUrl}/congressionaltrading";
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                request.Headers.Add("X-API-KEY", _apiKey);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Quiver API health check failed");
                return false;
            }
        }
        #endregion

        #region Private Helper Methods
        private async Task<T> MakeQuiverApiCallAsync<T>(string endpoint)
        {
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "your-quiver-api-key")
            {
                throw new InvalidOperationException("Quiver API key not configured");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Add("X-API-KEY", _apiKey);

            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            _logger.LogWarning("Quiver API call failed with status: {StatusCode}", response.StatusCode);
            return default(T);
        }

        #region Mock Data Generators
        private List<CongressionalTrading> GenerateMockCongressionalTradingData(int limit)
        {
            var data = new List<CongressionalTrading>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." },
                new { Ticker = "GOOGL", Company = "Alphabet Inc." },
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "META", Company = "Meta Platforms Inc." },
                new { Ticker = "NVDA", Company = "NVIDIA Corp." },
                new { Ticker = "JPM", Company = "JPMorgan Chase & Co." },
                new { Ticker = "JNJ", Company = "Johnson & Johnson" },
                new { Ticker = "V", Company = "Visa Inc." },
                new { Ticker = "PG", Company = "Procter & Gamble Co." }
            };

            var congresspeople = new[]
            {
                new { Name = "Nancy Pelosi", Party = "D", Chamber = "House", State = "CA" },
                new { Name = "Mitch McConnell", Party = "R", Chamber = "Senate", State = "KY" },
                new { Name = "Chuck Schumer", Party = "D", Chamber = "Senate", State = "NY" },
                new { Name = "Kevin McCarthy", Party = "R", Chamber = "House", State = "CA" },
                new { Name = "Alexandria Ocasio-Cortez", Party = "D", Chamber = "House", State = "NY" }
            };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var person = congresspeople[random.Next(congresspeople.Length)];
                var tradeType = random.Next(2) == 0 ? "BUY" : "SELL";
                var amount = random.Next(1000, 1000000);
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new CongressionalTrading
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    Congressperson = person.Name,
                    Party = person.Party,
                    Chamber = person.Chamber,
                    TradeType = tradeType,
                    Amount = amount,
                    Owner = random.Next(10) > 7 ? "Spouse" : "Self",
                    State = person.State
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private CongresspersonPortfolio GenerateMockCongresspersonPortfolio(string congresspersonName)
        {
            var random = new Random();
            var baseValue = random.Next(30000000, 150000000);
            var ytdReturn = random.Next(50, 200) / 10.0m;
            var ytdReturnAmount = baseValue * ytdReturn / 100;

            return new CongresspersonPortfolio
            {
                Name = congresspersonName,
                Party = random.Next(2) == 0 ? "D" : "R",
                State = "CA",
                Chamber = "House",
                TotalPortfolioValue = baseValue,
                YtdReturn = ytdReturn,
                YtdReturnAmount = ytdReturnAmount,
                TopHoldings = GenerateMockTopHoldings(3),
                RecentTrades = GenerateMockRecentTrades(3)
            };
        }

        private List<GovernmentContract> GenerateMockGovernmentContractsData(int limit)
        {
            var data = new List<GovernmentContract>();
            var companies = new[]
            {
                new { Ticker = "BA", Company = "Boeing Co." },
                new { Ticker = "LMT", Company = "Lockheed Martin Corp." },
                new { Ticker = "RTX", Company = "Raytheon Technologies Corp." },
                new { Ticker = "GD", Company = "General Dynamics Corp." },
                new { Ticker = "NOC", Company = "Northrop Grumman Corp." }
            };

            var agencies = new[] { "Department of Defense", "Department of Energy", "NASA", "Department of Homeland Security", "Department of Transportation" };
            var contractTypes = new[] { "Fixed Price", "Cost Plus", "Time & Materials", "IDIQ", "BPA" };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var agency = agencies[random.Next(agencies.Length)];
                var contractType = contractTypes[random.Next(contractTypes.Length)];
                var value = random.Next(100000, 100000000);
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new GovernmentContract
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    ContractValue = value,
                    Agency = agency,
                    ContractType = contractType,
                    Description = $"{contractType} contract for {agency} services"
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private List<HouseTrading> GenerateMockHouseTradingData(int limit)
        {
            var data = new List<HouseTrading>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." },
                new { Ticker = "GOOGL", Company = "Alphabet Inc." },
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "META", Company = "Meta Platforms Inc." }
            };

            var representatives = new[]
            {
                new { Name = "Kevin McCarthy", Party = "R", State = "CA" },
                new { Name = "Hakeem Jeffries", Party = "D", State = "NY" },
                new { Name = "Steve Scalise", Party = "R", State = "LA" },
                new { Name = "Katherine Clark", Party = "D", State = "MA" }
            };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var rep = representatives[random.Next(representatives.Length)];
                var tradeType = random.Next(2) == 0 ? "BUY" : "SELL";
                var amount = random.Next(1000, 1000000);
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new HouseTrading
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    Representative = rep.Name,
                    Party = rep.Party,
                    State = rep.State,
                    TradeType = tradeType,
                    Amount = amount
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private List<SenatorTrading> GenerateMockSenatorTradingData(int limit)
        {
            var data = new List<SenatorTrading>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." },
                new { Ticker = "GOOGL", Company = "Alphabet Inc." },
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "META", Company = "Meta Platforms Inc." }
            };

            var senators = new[]
            {
                new { Name = "Chuck Schumer", Party = "D", State = "NY" },
                new { Name = "Mitch McConnell", Party = "R", State = "KY" },
                new { Name = "Dick Durbin", Party = "D", State = "IL" },
                new { Name = "John Thune", Party = "R", State = "SD" }
            };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var senator = senators[random.Next(senators.Length)];
                var tradeType = random.Next(2) == 0 ? "BUY" : "SELL";
                var amount = random.Next(1000, 1000000);
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new SenatorTrading
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    Senator = senator.Name,
                    Party = senator.Party,
                    State = senator.State,
                    TradeType = tradeType,
                    Amount = amount
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private List<LobbyingActivity> GenerateMockLobbyingData(int limit)
        {
            var data = new List<LobbyingActivity>();
            var companies = new[]
            {
                new { Ticker = "XOM", Company = "Exxon Mobil Corp." },
                new { Ticker = "CVX", Company = "Chevron Corp." },
                new { Ticker = "JNJ", Company = "Johnson & Johnson" },
                new { Ticker = "PFE", Company = "Pfizer Inc." },
                new { Ticker = "BAC", Company = "Bank of America Corp." }
            };

            var lobbyingFirms = new[] { "Akin Gump", "Brownstein", "Holland & Knight", "Covington & Burling", "Squire Patton Boggs" };
            var issues = new[] { "Energy Policy", "Healthcare Reform", "Financial Regulation", "Tax Policy", "Trade Policy" };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var firm = lobbyingFirms[random.Next(lobbyingFirms.Length)];
                var issue = issues[random.Next(issues.Length)];
                var amount = random.Next(50000, 500000);
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new LobbyingActivity
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    LobbyingFirm = firm,
                    Amount = amount,
                    Issues = issue,
                    Registrant = company.Company
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private List<PatentMomentum> GenerateMockPatentMomentumData(int limit)
        {
            var data = new List<PatentMomentum>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc.", Industry = "Technology" },
                new { Ticker = "MSFT", Company = "Microsoft Corp.", Industry = "Technology" },
                new { Ticker = "GOOGL", Company = "Alphabet Inc.", Industry = "Technology" },
                new { Ticker = "TSLA", Company = "Tesla Inc.", Industry = "Automotive" },
                new { Ticker = "JNJ", Company = "Johnson & Johnson", Industry = "Healthcare" }
            };

            var patentTypes = new[] { "Utility", "Design", "Plant", "Software", "Biotechnology" };

            var random = new Random();
            for (int i = 0; i < limit; i++)
            {
                var company = companies[random.Next(companies.Length)];
                var patentType = patentTypes[random.Next(patentTypes.Length)];
                var patentCount = random.Next(10, 1000);
                var innovationScore = random.Next(50, 100) / 10.0m;
                var date = DateTime.Now.AddDays(-random.Next(365));

                data.Add(new PatentMomentum
                {
                    Date = date.ToString("MM/dd/yyyy"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    PatentCount = patentCount,
                    PatentType = patentType,
                    InnovationScore = innovationScore,
                    Industry = company.Industry
                });
            }

            return data.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private PortfolioSummary GenerateMockPortfolioSummary()
        {
            var random = new Random();
            var baseValue = random.Next(100000, 1000000);
            var returnPercentage = random.Next(-20, 30);
            var returnAmount = baseValue * returnPercentage / 100;
            
            // Generate today's P&L data
            var todayPnlPercentage = random.Next(-5, 8); // -5% to +8% daily range
            var todayPnl = baseValue * todayPnlPercentage / 100;

            return new PortfolioSummary
            {
                TotalPortfolioValue = baseValue,
                TotalReturn = returnAmount,
                ReturnPercentage = returnPercentage,
                TodayPnl = todayPnl,
                TodayPnlPercentage = todayPnlPercentage,
                LastUpdated = DateTime.Now,
                TopHoldings = GenerateMockTopHoldings(3),
                RecentTrades = GenerateMockRecentTrades(3)
            };
        }

        private AssetAllocation GenerateMockAssetAllocation(string breakdownType)
        {
            var random = new Random();
            var items = new List<AssetAllocationItem>();

            if (breakdownType == "asset_class")
            {
                var assetClasses = new[] { "Stocks", "Bonds", "Real Estate", "Commodities", "Cash" };
                var totalValue = 1000000m;
                var remainingValue = totalValue;

                for (int i = 0; i < assetClasses.Length - 1; i++)
                {
                    var value = random.Next(100000, (int)(remainingValue * 0.4m));
                    items.Add(new AssetAllocationItem
                    {
                        Label = assetClasses[i],
                        Value = value,
                        Percentage = value / totalValue * 100,
                        Color = GetRandomColor(random)
                    });
                    remainingValue -= value;
                }

                // Add remaining value to last item
                items.Add(new AssetAllocationItem
                {
                    Label = assetClasses[assetClasses.Length - 1],
                    Value = remainingValue,
                    Percentage = remainingValue / totalValue * 100,
                    Color = GetRandomColor(random)
                });
            }
            else if (breakdownType == "sector")
            {
                var sectors = new[] { "Technology", "Healthcare", "Financial", "Consumer", "Industrial", "Energy" };
                var totalValue = 1000000m;
                var remainingValue = totalValue;

                for (int i = 0; i < sectors.Length - 1; i++)
                {
                    var value = random.Next(100000, (int)(remainingValue * 0.4m));
                    items.Add(new AssetAllocationItem
                    {
                        Label = sectors[i],
                        Value = value,
                        Percentage = value / totalValue * 100,
                        Color = GetRandomColor(random)
                    });
                    remainingValue -= value;
                }

                items.Add(new AssetAllocationItem
                {
                    Label = sectors[sectors.Length - 1],
                    Value = remainingValue,
                    Percentage = remainingValue / totalValue * 100,
                    Color = GetRandomColor(random)
                });
            }
            else if (breakdownType == "performance")
            {
                var performanceRanges = new[] { "Negative Returns", "0-5% Returns", "6-12% Returns", "13-25% Returns", "25%+ Returns" };
                var totalValue = 1000000m;
                var remainingValue = totalValue;

                for (int i = 0; i < performanceRanges.Length - 1; i++)
                {
                    var value = random.Next(100000, (int)(remainingValue * 0.4m));
                    items.Add(new AssetAllocationItem
                    {
                        Label = performanceRanges[i],
                        Value = value,
                        Percentage = value / totalValue * 100,
                        Color = GetRandomColor(random)
                    });
                    remainingValue -= value;
                }

                items.Add(new AssetAllocationItem
                {
                    Label = performanceRanges[performanceRanges.Length - 1],
                    Value = remainingValue,
                    Percentage = remainingValue / totalValue * 100,
                    Color = GetRandomColor(random)
                });
            }

            return new AssetAllocation
            {
                BreakdownType = breakdownType,
                Items = items
            };
        }

        private List<PortfolioHolding> GenerateMockTopHoldings(int count)
        {
            var holdings = new List<PortfolioHolding>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." },
                new { Ticker = "GOOGL", Company = "Alphabet Inc." },
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "META", Company = "Meta Platforms Inc." }
            };

            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var company = companies[i % companies.Length];
                var value = random.Next(10000, 100000);
                var shares = random.Next(100, 1000);
                var returnPercentage = random.Next(-15, 25);

                holdings.Add(new PortfolioHolding
                {
                    Ticker = company.Ticker,
                    Company = company.Company,
                    Value = value,
                    Shares = shares,
                    Return = returnPercentage
                });
            }

            return holdings.OrderByDescending(x => x.Value).ToList();
        }

        private List<PortfolioTrade> GenerateMockRecentTrades(int count)
        {
            var trades = new List<PortfolioTrade>();
            var companies = new[]
            {
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." },
                new { Ticker = "GOOGL", Company = "Alphabet Inc." }
            };

            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var company = companies[i % companies.Length];
                var tradeType = random.Next(2) == 0 ? "BUY" : "SELL";
                var amount = random.Next(1000, 10000);
                var shares = random.Next(10, 100);
                var date = DateTime.Now.AddDays(-random.Next(30));

                trades.Add(new PortfolioTrade
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Ticker = company.Ticker,
                    Company = company.Company,
                    Type = tradeType,
                    Amount = amount,
                    Shares = shares
                });
            }

            return trades.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
        }

        private List<PortfolioDataPoint> GenerateMockPortfolioHistory(int months)
        {
            var data = new List<PortfolioDataPoint>();
            var random = new Random();
            var baseValue = 100000m;
            var currentDate = DateTime.Now.AddMonths(-months);

            for (int i = 0; i <= months; i++)
            {
                var volatility = (decimal)(random.NextDouble() - 0.5) * 0.1m; // ±5% volatility
                var growth = 0.02m; // 2% monthly growth
                var monthlyReturn = growth + volatility;
                
                baseValue *= (1 + monthlyReturn);
                var returnValue = baseValue - 100000m;

                data.Add(new PortfolioDataPoint
                {
                    Date = currentDate.AddMonths(i),
                    Value = baseValue,
                    Return = returnValue
                });
            }

            return data;
        }

        private string GetRandomColor(Random random)
        {
            var colors = new[] { "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", "#DDA0DD", "#98D8C8", "#F7DC6F" };
            return colors[random.Next(colors.Length)];
        }

        private RiskScore GenerateMockRiskScore()
        {
            var random = new Random();
            var currentScore = (decimal)(random.NextDouble() * 10); // 0-10 scale
            var previousScore = currentScore + (decimal)((random.NextDouble() - 0.5) * 2); // ±1 change
            var scoreChange = currentScore - previousScore;

            var riskCategory = currentScore switch
            {
                < 3 => "Low",
                < 7 => "Moderate",
                _ => "High"
            };

            var description = riskCategory switch
            {
                "Low" => "Conservative portfolio with minimal volatility",
                "Moderate" => "Balanced portfolio with moderate risk exposure",
                "High" => "Aggressive portfolio with high growth potential",
                _ => "Standard risk profile"
            };

            var riskFactors = new List<RiskFactor>
            {
                new RiskFactor { Name = "Market Volatility", Weight = 0.3m, Value = currentScore * 0.8m, Description = "Overall market risk exposure" },
                new RiskFactor { Name = "Sector Concentration", Weight = 0.25m, Value = currentScore * 0.9m, Description = "Diversification across sectors" },
                new RiskFactor { Name = "Geographic Exposure", Weight = 0.2m, Value = currentScore * 0.7m, Description = "International market exposure" },
                new RiskFactor { Name = "Liquidity", Weight = 0.15m, Value = currentScore * 0.6m, Description = "Ease of converting to cash" },
                new RiskFactor { Name = "Currency Risk", Weight = 0.1m, Value = currentScore * 0.5m, Description = "Foreign exchange exposure" }
            };

            return new RiskScore
            {
                Score = Math.Round(currentScore, 1),
                RiskCategory = riskCategory,
                PreviousScore = Math.Round(previousScore, 1),
                ScoreChange = Math.Round(scoreChange, 1),
                Description = description,
                RiskFactors = riskFactors,
                LastCalculated = DateTime.Now
            };
        }

        private DividendIncome GenerateMockDividendIncome()
        {
            var random = new Random();
            var monthlyAmount = random.Next(200, 500);
            var previousMonthAmount = monthlyAmount - random.Next(-50, 50);
            var changeAmount = monthlyAmount - previousMonthAmount;
            var changePercentage = previousMonthAmount > 0 ? (changeAmount / previousMonthAmount) * 100 : 0;

            var upcomingPayments = new List<DividendPayment>
            {
                new DividendPayment { Ticker = "AAPL", Company = "Apple Inc.", Amount = 87.50m, PaymentDate = DateTime.Now.AddDays(15), Status = "Upcoming" },
                new DividendPayment { Ticker = "MSFT", Company = "Microsoft Corp.", Amount = 76.00m, PaymentDate = DateTime.Now.AddDays(22), Status = "Upcoming" },
                new DividendPayment { Ticker = "JNJ", Company = "Johnson & Johnson", Amount = 45.68m, PaymentDate = DateTime.Now.AddDays(30), Status = "Upcoming" }
            };

            var recentPayments = new List<DividendPayment>
            {
                new DividendPayment { Ticker = "V", Company = "Visa Inc.", Amount = 32.00m, PaymentDate = DateTime.Now.AddDays(-5), Status = "Paid" },
                new DividendPayment { Ticker = "PG", Company = "Procter & Gamble Co.", Amount = 45.50m, PaymentDate = DateTime.Now.AddDays(-12), Status = "Paid" },
                new DividendPayment { Ticker = "KO", Company = "Coca-Cola Co.", Amount = 28.00m, PaymentDate = DateTime.Now.AddDays(-18), Status = "Paid" }
            };

            return new DividendIncome
            {
                MonthlyAmount = monthlyAmount,
                PreviousMonthAmount = previousMonthAmount,
                ChangeAmount = changeAmount,
                ChangePercentage = Math.Round(changePercentage, 2),
                NextPaymentDate = DateTime.Now.AddDays(15),
                UpcomingPayments = upcomingPayments,
                RecentPayments = recentPayments
            };
        }

        private TopPerformer GenerateMockTopPerformer()
        {
            var random = new Random();
            var companies = new[]
            {
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "NVDA", Company = "NVIDIA Corp." },
                new { Ticker = "META", Company = "Meta Platforms Inc." },
                new { Ticker = "AAPL", Company = "Apple Inc." },
                new { Ticker = "MSFT", Company = "Microsoft Corp." }
            };

            var company = companies[random.Next(companies.Length)];
            var returnPercentage = random.Next(5, 25);
            var shares = random.Next(10, 100);
            var currentValue = random.Next(2000, 8000);
            var returnAmount = (currentValue * returnPercentage) / 100;

            return new TopPerformer
            {
                Ticker = company.Ticker,
                Company = company.Company,
                ReturnAmount = Math.Round(returnAmount, 2),
                ReturnPercentage = returnPercentage,
                Shares = shares,
                CurrentValue = currentValue,
                LastUpdated = DateTime.Now
            };
        }
        #endregion
        #endregion
    }
}
