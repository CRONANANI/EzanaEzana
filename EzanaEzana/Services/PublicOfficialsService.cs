using EzanaEzana.Models;
using EzanaEzana.Models.PublicOfficials;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace EzanaEzana.Services
{
    public class PublicOfficialsService : IPublicOfficialsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PublicOfficialsService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly int _timeoutSeconds;

        public PublicOfficialsService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<PublicOfficialsService> logger)
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

        #region Historical Congress Trading Card
        public async Task<HistoricalCongressTradingCard> GetHistoricalCongressTradingCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockHistoricalCongressTradingCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/congressionaltrading";
                var response = await MakeQuiverApiCallAsync<List<CongressionalTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildHistoricalCongressTradingCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch congressional trading data from Quiver API, using mock data");
            }

            return GenerateMockHistoricalCongressTradingCard();
        }

        private async Task<HistoricalCongressTradingCard> BuildHistoricalCongressTradingCardFromQuiverAsync(List<CongressionalTrading> tradingData)
        {
            var card = new HistoricalCongressTradingCard
            {
                TotalTrades = tradingData.Count,
                TotalVolume = tradingData.Sum(t => t.Amount),
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = tradingData.Select(t => t.Congressperson).Distinct().Count(),
                UniqueCompanies = tradingData.Select(t => t.Ticker).Distinct().Count(),
                RecentTrades = tradingData
                    .OrderByDescending(t => DateTime.Parse(t.Date))
                    .Take(10)
                    .Select(t => new CongressTrade
                    {
                        Date = DateTime.Parse(t.Date),
                        Ticker = t.Ticker,
                        Company = t.Company,
                        Congressperson = t.Congressperson,
                        Party = t.Party,
                        Chamber = t.Chamber,
                        TradeType = t.TradeType,
                        TradeValue = t.Amount,
                        State = t.State
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Government Contracts Card
        public async Task<GovernmentContractsCard> GetGovernmentContractsCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockGovernmentContractsCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/governmentcontracts";
                var response = await MakeQuiverApiCallAsync<List<GovernmentContract>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildGovernmentContractsCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch government contracts from Quiver API, using mock data");
            }

            return GenerateMockGovernmentContractsCard();
        }

        private async Task<GovernmentContractsCard> BuildGovernmentContractsCardFromQuiverAsync(List<GovernmentContract> contractsData)
        {
            var card = new GovernmentContractsCard
            {
                TotalContracts = contractsData.Count,
                TotalValue = contractsData.Sum(c => c.ContractValue),
                LastUpdated = DateTime.UtcNow,
                ActiveCompanies = contractsData.Select(c => c.Ticker).Distinct().Count(),
                RecentContracts = contractsData
                    .OrderByDescending(c => DateTime.Parse(c.Date))
                    .Take(10)
                    .Select(c => new GovernmentContractItem
                    {
                        Date = DateTime.Parse(c.Date),
                        Ticker = c.Ticker,
                        Company = c.Company,
                        ContractValue = c.ContractValue,
                        Agency = c.Agency,
                        ContractType = c.ContractType,
                        Description = c.Description
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region House Trading Card
        public async Task<HouseTradingCard> GetHouseTradingCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockHouseTradingCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/housetrading";
                var response = await MakeQuiverApiCallAsync<List<HouseTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildHouseTradingCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch house trading from Quiver API, using mock data");
            }

            return GenerateMockHouseTradingCard();
        }

        private async Task<HouseTradingCard> BuildHouseTradingCardFromQuiverAsync(List<HouseTrading> tradingData)
        {
            var card = new HouseTradingCard
            {
                TotalTrades = tradingData.Count,
                TotalVolume = tradingData.Sum(t => t.Amount),
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = tradingData.Select(t => t.Representative).Distinct().Count(),
                UniqueCompanies = tradingData.Select(t => t.Ticker).Distinct().Count(),
                RecentTrades = tradingData
                    .OrderByDescending(t => DateTime.Parse(t.Date))
                    .Take(10)
                    .Select(t => new HouseTrade
                    {
                        Date = DateTime.Parse(t.Date),
                        Ticker = t.Ticker,
                        Company = t.Company,
                        Representative = t.Representative,
                        Party = t.Party,
                        State = t.State,
                        TradeType = t.TradeType,
                        TradeValue = t.Amount
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Lobbying Activity Card
        public async Task<LobbyingActivityCard> GetLobbyingActivityCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockLobbyingActivityCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/lobbying";
                var response = await MakeQuiverApiCallAsync<List<LobbyingActivity>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildLobbyingActivityCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch lobbying activity from Quiver API, using mock data");
            }

            return GenerateMockLobbyingActivityCard();
        }

        private async Task<LobbyingActivityCard> BuildLobbyingActivityCardFromQuiverAsync(List<LobbyingActivity> lobbyingData)
        {
            var card = new LobbyingActivityCard
            {
                TotalReports = lobbyingData.Count,
                TotalSpending = lobbyingData.Sum(l => l.Amount),
                LastUpdated = DateTime.UtcNow,
                ActiveFirms = lobbyingData.Select(l => l.Registrant).Distinct().Count(),
                RecentActivities = lobbyingData
                    .OrderByDescending(l => DateTime.Parse(l.ReportYear))
                    .Take(10)
                    .Select(l => new LobbyingActivityItem
                    {
                        ReportYear = l.ReportYear,
                        Registrant = l.Registrant,
                        Client = l.Client,
                        Amount = l.Amount,
                        Issue = l.Issue,
                        Type = l.Type
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Senator Trading Card
        public async Task<SenatorTradingCard> GetSenatorTradingCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockSenatorTradingCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/senatortrading";
                var response = await MakeQuiverApiCallAsync<List<SenatorTrading>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildSenatorTradingCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch senator trading from Quiver API, using mock data");
            }

            return GenerateMockSenatorTradingCard();
        }

        private async Task<SenatorTradingCard> BuildSenatorTradingCardFromQuiverAsync(List<SenatorTrading> tradingData)
        {
            var card = new SenatorTradingCard
            {
                TotalTrades = tradingData.Count,
                TotalVolume = tradingData.Sum(t => t.Amount),
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = tradingData.Select(t => t.Senator).Distinct().Count(),
                UniqueCompanies = tradingData.Select(t => t.Ticker).Distinct().Count(),
                RecentTrades = tradingData
                    .OrderByDescending(t => DateTime.Parse(t.Date))
                    .Take(10)
                    .Select(t => new SenatorTrade
                    {
                        Date = DateTime.Parse(t.Date),
                        Ticker = t.Ticker,
                        Company = t.Company,
                        Senator = t.Senator,
                        Party = t.Party,
                        State = t.State,
                        TradeType = t.TradeType,
                        TradeValue = t.Amount
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Patent Momentum Card
        public async Task<PatentMomentumCard> GetPatentMomentumCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockPatentMomentumCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/patentmomentum";
                var response = await MakeQuiverApiCallAsync<List<PatentMomentum>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildPatentMomentumCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch patent momentum from Quiver API, using mock data");
            }

            return GenerateMockPatentMomentumCard();
        }

        private async Task<PatentMomentumCard> BuildPatentMomentumCardFromQuiverAsync(List<PatentMomentum> patentData)
        {
            var card = new PatentMomentumCard
            {
                TotalPatents = patentData.Count,
                ActivePatents = patentData.Count(p => p.Status == "Active"),
                PendingPatents = patentData.Count(p => p.Status == "Pending"),
                LastUpdated = DateTime.UtcNow,
                RecentPatents = patentData
                    .OrderByDescending(p => DateTime.Parse(p.Date))
                    .Take(10)
                    .Select(p => new PatentItem
                    {
                        Date = DateTime.Parse(p.Date),
                        Company = p.Company,
                        Title = p.Title,
                        Status = p.Status,
                        Technology = p.Technology,
                        Inventor = p.Inventor
                    })
                    .ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Market Sentiment Card
        public async Task<MarketSentimentCard> GetMarketSentimentCardAsync(bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockMarketSentimentCard();
            }

            try
            {
                var endpoint = $"{_baseUrl}/marketintelligence";
                var response = await MakeQuiverApiCallAsync<Dictionary<string, MarketIntelligenceData>>(endpoint);
                
                if (response != null && response.Any())
                {
                    return await BuildMarketSentimentCardFromQuiverAsync(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch market intelligence from Quiver API, using mock data");
            }

            return GenerateMockMarketSentimentCard();
        }

        private async Task<MarketSentimentCard> BuildMarketSentimentCardFromQuiverAsync(Dictionary<string, MarketIntelligenceData> marketData)
        {
            var card = new MarketSentimentCard
            {
                OverallSentiment = "Neutral",
                SentimentScore = 50,
                LastUpdated = DateTime.UtcNow,
                MarketIndicators = marketData.Select(kvp => new MarketIndicator
                {
                    Name = kvp.Key,
                    Value = kvp.Value.Value,
                    Trend = kvp.Value.Trend,
                    Impact = kvp.Value.Impact
                }).ToList()
            };

            card.CalculateMetrics();
            return card;
        }
        #endregion

        #region Public Officials Summary
        public async Task<PublicOfficialsSummary> GetPublicOfficialsSummaryAsync(bool useMockData = false)
        {
            var summary = new PublicOfficialsSummary
            {
                CongressTrading = await GetHistoricalCongressTradingCardAsync(useMockData),
                GovernmentContracts = await GetGovernmentContractsCardAsync(useMockData),
                HouseTrading = await GetHouseTradingCardAsync(useMockData),
                LobbyingActivity = await GetLobbyingActivityCardAsync(useMockData),
                SenatorTrading = await GetSenatorTradingCardAsync(useMockData),
                PatentMomentum = await GetPatentMomentumCardAsync(useMockData),
                MarketSentiment = await GetMarketSentimentCardAsync(useMockData),
                LastRefreshed = DateTime.UtcNow
            };

            summary.CalculateSummaryMetrics();
            return summary;
        }
        #endregion

        #region Utility Methods
        public async Task<bool> HasQuiverApiAccessAsync()
        {
            return !string.IsNullOrEmpty(_apiKey) && !string.IsNullOrEmpty(_baseUrl);
        }

        public async Task<string> GetQuiverApiKeyAsync()
        {
            return _apiKey ?? string.Empty;
        }

        public async Task<bool> RefreshFromQuiverAsync()
        {
            try
            {
                var healthCheck = await CheckQuiverApiHealthAsync();
                if (healthCheck)
                {
                    _logger.LogInformation("Successfully refreshed data from Quiver API");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh data from Quiver API");
            }
            return false;
        }

        public async Task<Dictionary<string, object>> GetMockPublicOfficialsData()
        {
            return new Dictionary<string, object>
            {
                ["HistoricalCongressTrading"] = GenerateMockHistoricalCongressTradingCard(),
                ["GovernmentContracts"] = GenerateMockGovernmentContractsCard(),
                ["HouseTrading"] = GenerateMockHouseTradingCard(),
                ["LobbyingActivity"] = GenerateMockLobbyingActivityCard(),
                ["SenatorTrading"] = GenerateMockSenatorTradingCard(),
                ["PatentMomentum"] = GenerateMockPatentMomentumCard(),
                ["MarketSentiment"] = GenerateMockMarketSentimentCard()
            };
        }

        public async Task<bool> CheckQuiverApiHealthAsync()
        {
            try
            {
                var endpoint = $"{_baseUrl}/health";
                var response = await _httpClient.GetAsync(endpoint);
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
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<QuiverApiResponse<T>>(content);
                    return result?.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Quiver API endpoint: {Endpoint}", endpoint);
            }
            return default(T);
        }

        #region Mock Data Generation
        private HistoricalCongressTradingCard GenerateMockHistoricalCongressTradingCard()
        {
            var card = new HistoricalCongressTradingCard
            {
                TotalTrades = 1250,
                TotalVolume = 45000000,
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = 45,
                UniqueCompanies = 89,
                RecentTrades = GenerateMockCongressTrades(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private GovernmentContractsCard GenerateMockGovernmentContractsCard()
        {
            var card = new GovernmentContractsCard
            {
                TotalContracts = 567,
                TotalValue = 1250000000,
                LastUpdated = DateTime.UtcNow,
                ActiveCompanies = 234,
                RecentContracts = GenerateMockGovernmentContracts(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private HouseTradingCard GenerateMockHouseTradingCard()
        {
            var card = new HouseTradingCard
            {
                TotalTrades = 890,
                TotalVolume = 32000000,
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = 67,
                UniqueCompanies = 156,
                RecentTrades = GenerateMockHouseTrades(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private LobbyingActivityCard GenerateMockLobbyingActivityCard()
        {
            var card = new LobbyingActivityCard
            {
                TotalReports = 1234,
                TotalSpending = 89000000,
                LastUpdated = DateTime.UtcNow,
                ActiveFirms = 89,
                RecentActivities = GenerateMockLobbyingActivities(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private SenatorTradingCard GenerateMockSenatorTradingCard()
        {
            var card = new SenatorTradingCard
            {
                TotalTrades = 456,
                TotalVolume = 18000000,
                LastUpdated = DateTime.UtcNow,
                ActiveTraders = 23,
                UniqueCompanies = 78,
                RecentTrades = GenerateMockSenatorTrades(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private PatentMomentumCard GenerateMockPatentMomentumCard()
        {
            var card = new PatentMomentumCard
            {
                TotalPatents = 2345,
                ActivePatents = 1890,
                PendingPatents = 455,
                LastUpdated = DateTime.UtcNow,
                RecentPatents = GenerateMockPatents(10)
            };

            card.CalculateMetrics();
            return card;
        }

        private MarketSentimentCard GenerateMockMarketSentimentCard()
        {
            var card = new MarketSentimentCard
            {
                OverallSentiment = "Bullish",
                SentimentScore = 72,
                LastUpdated = DateTime.UtcNow,
                MarketIndicators = GenerateMockMarketIndicators()
            };

            card.CalculateMetrics();
            return card;
        }

        private List<CongressTrade> GenerateMockCongressTrades(int count)
        {
            var trades = new List<CongressTrade>();
            var companies = new[] { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "META", "NVDA", "NFLX", "AMD", "INTC" };
            var congresspeople = new[] { "John Smith", "Jane Doe", "Bob Johnson", "Alice Brown", "Charlie Wilson" };
            var parties = new[] { "D", "R", "I" };
            var chambers = new[] { "House", "Senate" };
            var tradeTypes = new[] { "Buy", "Sell" };

            for (int i = 0; i < count; i++)
            {
                trades.Add(new CongressTrade
                {
                    Date = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                    Ticker = companies[Random.Shared.Next(companies.Length)],
                    Company = $"Company {companies[Random.Shared.Next(companies.Length)]}",
                    Congressperson = congresspeople[Random.Shared.Next(congresspeople.Length)],
                    Party = parties[Random.Shared.Next(parties.Length)],
                    Chamber = chambers[Random.Shared.Next(chambers.Length)],
                    TradeType = tradeTypes[Random.Shared.Next(tradeTypes.Length)],
                    TradeValue = Random.Shared.Next(10000, 1000000),
                    State = "CA"
                });
            }

            return trades;
        }

        private List<GovernmentContractItem> GenerateMockGovernmentContracts(int count)
        {
            var contracts = new List<GovernmentContractItem>();
            var companies = new[] { "LOCK", "RTX", "BA", "GD", "LMT", "NOC", "HII", "TDG", "AJRD", "KTOS" };
            var agencies = new[] { "DOD", "DOE", "NASA", "DHS", "DOJ" };
            var contractTypes = new[] { "Research", "Development", "Production", "Maintenance", "Services" };

            for (int i = 0; i < count; i++)
            {
                contracts.Add(new GovernmentContractItem
                {
                    Date = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 90)),
                    Ticker = companies[Random.Shared.Next(companies.Length)],
                    Company = $"Company {companies[Random.Shared.Next(companies.Length)]}",
                    ContractValue = Random.Shared.Next(100000, 10000000),
                    Agency = agencies[Random.Shared.Next(agencies.Length)],
                    ContractType = contractTypes[Random.Shared.Next(contractTypes.Length)],
                    Description = $"Contract for {contractTypes[Random.Shared.Next(contractTypes.Length)]} services"
                });
            }

            return contracts;
        }

        private List<HouseTrade> GenerateMockHouseTrades(int count)
        {
            var trades = new List<HouseTrade>();
            var companies = new[] { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "META", "NVDA", "NFLX", "AMD", "INTC" };
            var representatives = new[] { "Rep. Smith", "Rep. Johnson", "Rep. Brown", "Rep. Davis", "Rep. Wilson" };
            var parties = new[] { "D", "R" };
            var states = new[] { "CA", "TX", "NY", "FL", "IL" };
            var tradeTypes = new[] { "Buy", "Sell" };

            for (int i = 0; i < count; i++)
            {
                trades.Add(new HouseTrade
                {
                    Date = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                    Ticker = companies[Random.Shared.Next(companies.Length)],
                    Company = $"Company {companies[Random.Shared.Next(companies.Length)]}",
                    Representative = representatives[Random.Shared.Next(representatives.Length)],
                    Party = parties[Random.Shared.Next(parties.Length)],
                    State = states[Random.Shared.Next(states.Length)],
                    TradeType = tradeTypes[Random.Shared.Next(tradeTypes.Length)],
                    TradeValue = Random.Shared.Next(5000, 500000)
                });
            }

            return trades;
        }

        private List<LobbyingActivityItem> GenerateMockLobbyingActivities(int count)
        {
            var activities = new List<LobbyingActivityItem>();
            var registrants = new[] { "Lobbying Firm A", "Lobbying Firm B", "Lobbying Firm C", "Lobbying Firm D" };
            var clients = new[] { "Tech Corp", "Pharma Inc", "Energy Co", "Bank Corp", "Auto Corp" };
            var issues = new[] { "Tax Reform", "Healthcare", "Climate Change", "Financial Regulation", "Infrastructure" };
            var types = new[] { "Direct", "Grassroots", "Coalition" };

            for (int i = 0; i < count; i++)
            {
                activities.Add(new LobbyingActivityItem
                {
                    ReportYear = (DateTime.UtcNow.Year - Random.Shared.Next(0, 3)).ToString(),
                    Registrant = registrants[Random.Shared.Next(registrants.Length)],
                    Client = clients[Random.Shared.Next(clients.Length)],
                    Amount = Random.Shared.Next(100000, 5000000),
                    Issue = issues[Random.Shared.Next(issues.Length)],
                    Type = types[Random.Shared.Next(types.Length)]
                });
            }

            return activities;
        }

        private List<SenatorTrade> GenerateMockSenatorTrades(int count)
        {
            var trades = new List<SenatorTrade>();
            var companies = new[] { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "META", "NVDA", "NFLX", "AMD", "INTC" };
            var senators = new[] { "Sen. Smith", "Sen. Johnson", "Sen. Brown", "Sen. Davis", "Sen. Wilson" };
            var parties = new[] { "D", "R", "I" };
            var states = new[] { "CA", "TX", "NY", "FL", "IL" };
            var tradeTypes = new[] { "Buy", "Sell" };

            for (int i = 0; i < count; i++)
            {
                trades.Add(new SenatorTrade
                {
                    Date = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                    Ticker = companies[Random.Shared.Next(companies.Length)],
                    Company = $"Company {companies[Random.Shared.Next(companies.Length)]}",
                    Senator = senators[Random.Shared.Next(senators.Length)],
                    Party = parties[Random.Shared.Next(parties.Length)],
                    State = states[Random.Shared.Next(states.Length)],
                    TradeType = tradeTypes[Random.Shared.Next(tradeTypes.Length)],
                    TradeValue = Random.Shared.Next(10000, 1000000)
                });
            }

            return trades;
        }

        private List<PatentItem> GenerateMockPatents(int count)
        {
            var patents = new List<PatentItem>();
            var companies = new[] { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "META", "NVDA", "NFLX", "AMD", "INTC" };
            var technologies = new[] { "AI/ML", "Blockchain", "IoT", "Cybersecurity", "Cloud Computing", "Quantum Computing" };
            var inventors = new[] { "Dr. Smith", "Dr. Johnson", "Dr. Brown", "Dr. Davis", "Dr. Wilson" };
            var statuses = new[] { "Active", "Pending", "Expired" };

            for (int i = 0; i < count; i++)
            {
                patents.Add(new PatentItem
                {
                    Date = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 365)),
                    Company = companies[Random.Shared.Next(companies.Length)],
                    Title = $"Innovative {technologies[Random.Shared.Next(technologies.Length)]} Solution",
                    Status = statuses[Random.Shared.Next(statuses.Length)],
                    Technology = technologies[Random.Shared.Next(technologies.Length)],
                    Inventor = inventors[Random.Shared.Next(inventors.Length)]
                });
            }

            return patents;
        }

        private List<MarketIndicator> GenerateMockMarketIndicators()
        {
            return new List<MarketIndicator>
            {
                new MarketIndicator { Name = "VIX", Value = 18.5, Trend = "Decreasing", Impact = "Low" },
                new MarketIndicator { Name = "S&P 500", Value = 4500, Trend = "Increasing", Impact = "High" },
                new MarketIndicator { Name = "Treasury Yield", Value = 4.2, Trend = "Stable", Impact = "Medium" },
                new MarketIndicator { Name = "Oil Price", Value = 75.30, Trend = "Increasing", Impact = "Medium" }
            };
        }
        #endregion
        #endregion
    }
}
