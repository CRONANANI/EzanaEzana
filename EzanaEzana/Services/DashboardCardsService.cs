using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.Models.DashboardCards;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Services
{
    /// <summary>
    /// Service implementation for dashboard cards data
    /// </summary>
    public class DashboardCardsService : IDashboardCardsService
    {
        private readonly ILogger<DashboardCardsService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPlaidService _plaidService;
        private readonly IQuiverService _quiverService;

        public DashboardCardsService(
            ILogger<DashboardCardsService> logger,
            ApplicationDbContext context,
            IPlaidService plaidService,
            IQuiverService quiverService)
        {
            _logger = logger;
            _context = context;
            _plaidService = plaidService;
            _quiverService = quiverService;
        }

        public async Task<PortfolioValueCard> GetPortfolioValueCardAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockPortfolioValueCard();
            }

            try
            {
                // Check if user has Plaid accounts
                if (!await HasPlaidAccountsAsync(userId))
                {
                    _logger.LogInformation("User {UserId} has no Plaid accounts, using mock data", userId);
                    return GenerateMockPortfolioValueCard();
                }

                // Get portfolio data from Plaid
                var portfolioData = await GetPortfolioDataFromPlaidAsync(userId);
                return portfolioData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio value card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockPortfolioValueCard();
            }
        }

        public async Task<TodaysPnlCard> GetTodaysPnlCardAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockTodaysPnlCard();
            }

            try
            {
                if (!await HasPlaidAccountsAsync(userId))
                {
                    return GenerateMockTodaysPnlCard();
                }

                var pnlData = await GetPnlDataFromPlaidAsync(userId);
                return pnlData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting today's P&L card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockTodaysPnlCard();
            }
        }

        public async Task<TopPerformerCard> GetTopPerformerCardAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockTopPerformerCard();
            }

            try
            {
                if (!await HasPlaidAccountsAsync(userId))
                {
                    return GenerateMockTopPerformerCard();
                }

                var topPerformerData = await GetTopPerformerFromPlaidAsync(userId);
                return topPerformerData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top performer card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockTopPerformerCard();
            }
        }

        public async Task<RiskScoreCard> GetRiskScoreCardAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockRiskScoreCard();
            }

            try
            {
                if (!await HasPlaidAccountsAsync(userId))
                {
                    return GenerateMockRiskScoreCard();
                }

                var riskData = await CalculateRiskScoreFromPlaidAsync(userId);
                return riskData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting risk score card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockRiskScoreCard();
            }
        }

        public async Task<MonthlyDividendsCard> GetMonthlyDividendsCardAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockMonthlyDividendsCard();
            }

            try
            {
                if (!await HasPlaidAccountsAsync(userId))
                {
                    return GenerateMockMonthlyDividendsCard();
                }

                var dividendData = await GetDividendDataFromPlaidAsync(userId);
                return dividendData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly dividends card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockMonthlyDividendsCard();
            }
        }

        public async Task<AssetAllocationCard> GetAssetAllocationCardAsync(string userId, string breakdownType = "asset_class", bool useMockData = false)
        {
            if (useMockData)
            {
                return GenerateMockAssetAllocationCard(breakdownType);
            }

            try
            {
                if (!await HasPlaidAccountsAsync(userId))
                {
                    return GenerateMockAssetAllocationCard(breakdownType);
                }

                var allocationData = await GetAssetAllocationFromPlaidAsync(userId, breakdownType);
                return allocationData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset allocation card data for user {UserId}, falling back to mock data", userId);
                return GenerateMockAssetAllocationCard(breakdownType);
            }
        }

        public async Task<DashboardCardsSummary> GetDashboardSummaryAsync(string userId, bool useMockData = false)
        {
            if (useMockData)
            {
                return GetMockDashboardData();
            }

            try
            {
                var summary = new DashboardCardsSummary
                {
                    PortfolioValue = await GetPortfolioValueCardAsync(userId, false),
                    TodaysPnl = await GetTodaysPnlCardAsync(userId, false),
                    TopPerformer = await GetTopPerformerCardAsync(userId, false),
                    RiskScore = await GetRiskScoreCardAsync(userId, false),
                    MonthlyDividends = await GetMonthlyDividendsCardAsync(userId, false),
                    AssetAllocation = await GetAssetAllocationCardAsync(userId, "asset_class", false),
                    LastRefreshed = DateTime.Now
                };

                // Calculate portfolio health score
                summary.CalculatePortfolioHealthScore();

                // Generate insights and alerts
                summary.Insights = await GenerateInsightsAsync(userId, summary);
                summary.Alerts = await GenerateAlertsAsync(userId, summary);

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard summary for user {UserId}, falling back to mock data", userId);
                return GetMockDashboardData();
            }
        }

        public async Task<bool> HasPlaidAccountsAsync(string userId)
        {
            try
            {
                var hasAccounts = await _context.PlaidItems
                    .AnyAsync(item => item.UserId == userId && item.IsActive);

                return hasAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Plaid accounts for user {UserId}", userId);
                return false;
            }
        }

        public async Task<List<string>> GetUserPlaidAccessTokensAsync(string userId)
        {
            try
            {
                var accessTokens = await _context.PlaidItems
                    .Where(item => item.UserId == userId && item.IsActive)
                    .Select(item => item.PlaidAccessToken)
                    .ToListAsync();

                return accessTokens;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Plaid access tokens for user {UserId}", userId);
                return new List<string>();
            }
        }

        public async Task<bool> RefreshFromPlaidAsync(string userId)
        {
            try
            {
                // Sync accounts and transactions from Plaid
                await _plaidService.SyncAccountsAsync(userId);
                await _plaidService.SyncTransactionsAsync(userId, DateTime.Now.AddDays(-30), DateTime.Now);

                _logger.LogInformation("Successfully refreshed data from Plaid for user {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing data from Plaid for user {UserId}", userId);
                return false;
            }
        }

        public DashboardCardsSummary GetMockDashboardData()
        {
            return new DashboardCardsSummary
            {
                PortfolioValue = GenerateMockPortfolioValueCard(),
                TodaysPnl = GenerateMockTodaysPnlCard(),
                TopPerformer = GenerateMockTopPerformerCard(),
                RiskScore = GenerateMockRiskScoreCard(),
                MonthlyDividends = GenerateMockMonthlyDividendsCard(),
                AssetAllocation = GenerateMockAssetAllocationCard("asset_class"),
                LastRefreshed = DateTime.Now
            };
        }

        #region Plaid Data Retrieval Methods

        private async Task<PortfolioValueCard> GetPortfolioDataFromPlaidAsync(string userId)
        {
            var accessTokens = await GetUserPlaidAccessTokensAsync(userId);
            var totalValue = 0m;
            var accounts = new List<PlaidAccount>();

            foreach (var accessToken in accessTokens)
            {
                var plaidAccounts = await _plaidService.GetAccountsAsync(accessToken);
                foreach (var account in plaidAccounts)
                {
                    if (account.CurrentBalance.HasValue)
                    {
                        totalValue += account.CurrentBalance.Value;
                    }
                }
                accounts.AddRange(plaidAccounts);
            }

            // Calculate returns (this would need historical data from Plaid)
            var returnPercentage = 0m; // Placeholder
            var totalReturn = totalValue * returnPercentage / 100;

            return new PortfolioValueCard
            {
                TotalValue = totalValue,
                TotalReturn = totalReturn,
                ReturnPercentage = returnPercentage,
                TodayPnl = 0, // Would need real-time price data
                TodayPnlPercentage = 0,
                LastUpdated = DateTime.Now,
                TopHoldings = new List<PortfolioHolding>(), // Would need holdings data
                RecentTrades = new List<PortfolioTrade>(), // Would need transaction data
                PerformanceHistory = new List<PortfolioDataPoint>()
            };
        }

        private async Task<TodaysPnlCard> GetPnlDataFromPlaidAsync(string userId)
        {
            // This would require real-time price data from a market data provider
            // For now, return mock data structure
            return GenerateMockTodaysPnlCard();
        }

        private async Task<TopPerformerCard> GetTopPerformerFromPlaidAsync(string userId)
        {
            // This would require holdings and performance data
            // For now, return mock data structure
            return GenerateMockTopPerformerCard();
        }

        private async Task<RiskScoreCard> CalculateRiskScoreFromPlaidAsync(string userId)
        {
            // This would require portfolio analysis based on holdings
            // For now, return mock data structure
            return GenerateMockRiskScoreCard();
        }

        private async Task<MonthlyDividendsCard> GetDividendDataFromPlaidAsync(string userId)
        {
            // This would require dividend data from a separate service
            // For now, return mock data structure
            return GenerateMockMonthlyDividendsCard();
        }

        private async Task<AssetAllocationCard> GetAssetAllocationFromPlaidAsync(string userId, string breakdownType)
        {
            var accessTokens = await GetUserPlaidAccessTokensAsync(userId);
            var allocationItems = new List<AssetAllocationItem>();

            foreach (var accessToken in accessTokens)
            {
                var accounts = await _plaidService.GetAccountsAsync(accessToken);
                var totalValue = accounts.Sum(a => a.CurrentBalance ?? 0);

                if (totalValue > 0)
                {
                    foreach (var account in accounts)
                    {
                        if (account.CurrentBalance.HasValue)
                        {
                            var percentage = (account.CurrentBalance.Value / totalValue) * 100;
                            allocationItems.Add(new AssetAllocationItem
                            {
                                Label = account.AccountType ?? "Unknown",
                                Value = account.CurrentBalance.Value,
                                Percentage = percentage,
                                Color = GetRandomColor()
                            });
                        }
                    }
                }
            }

            return new AssetAllocationCard
            {
                BreakdownType = breakdownType,
                Items = allocationItems,
                TotalPortfolioValue = allocationItems.Sum(i => i.Value),
                DiversificationScore = CalculateDiversificationScore(allocationItems),
                RiskAssessment = "Based on account types",
                LastRebalancingDate = null,
                TargetAllocations = new Dictionary<string, decimal>(),
                AllocationDeviations = new Dictionary<string, decimal>()
            };
        }

        #endregion

        #region Mock Data Generation Methods

        private PortfolioValueCard GenerateMockPortfolioValueCard()
        {
            var random = new Random();
            var baseValue = random.Next(100000, 1000000);
            var returnPercentage = random.Next(-20, 30);
            var returnAmount = baseValue * returnPercentage / 100;
            var todayPnlPercentage = random.Next(-5, 8);
            var todayPnl = baseValue * todayPnlPercentage / 100;

            return new PortfolioValueCard
            {
                TotalValue = baseValue,
                TotalReturn = returnAmount,
                ReturnPercentage = returnPercentage,
                TodayPnl = todayPnl,
                TodayPnlPercentage = todayPnlPercentage,
                LastUpdated = DateTime.Now,
                TopHoldings = GenerateMockTopHoldings(3),
                RecentTrades = GenerateMockRecentTrades(3),
                PerformanceHistory = GenerateMockPerformanceHistory(12)
            };
        }

        private TodaysPnlCard GenerateMockTodaysPnlCard()
        {
            var random = new Random();
            var todayPnl = random.Next(-5000, 10000);
            var todayPnlPercentage = random.Next(-5, 8);
            var totalValue = random.Next(100000, 1000000);

            return new TodaysPnlCard
            {
                TodayPnl = todayPnl,
                TodayPnlPercentage = todayPnlPercentage,
                TotalPortfolioValue = totalValue,
                LastUpdated = DateTime.Now,
                MarketStatus = GetRandomMarketStatus(),
                PreviousDayPnl = todayPnl - random.Next(-2000, 2000),
                WeekToDatePnl = todayPnl + random.Next(-5000, 5000),
                MonthToDatePnl = todayPnl + random.Next(-10000, 10000)
            };
        }

        private TopPerformerCard GenerateMockTopPerformerCard()
        {
            var random = new Random();
            var companies = new[]
            {
                new { Ticker = "TSLA", Company = "Tesla Inc." },
                new { Ticker = "NVDA", Company = "NVIDIA Corp." },
                new { Ticker = "META", Company = "Meta Platforms Inc." }
            };

            var company = companies[random.Next(companies.Length)];
            var returnPercentage = random.Next(5, 25);
            var shares = random.Next(10, 100);
            var currentValue = random.Next(2000, 8000);
            var returnAmount = (currentValue * returnPercentage) / 100;

            return new TopPerformerCard
            {
                Ticker = company.Ticker,
                Company = company.Company,
                ReturnAmount = returnAmount,
                ReturnPercentage = returnPercentage,
                Shares = shares,
                CurrentValue = currentValue,
                LastUpdated = DateTime.Now,
                Sector = "Technology",
                PreviousDayReturn = returnPercentage - random.Next(-5, 5),
                WeekToDateReturn = returnPercentage + random.Next(-10, 10),
                MonthToDateReturn = returnPercentage + random.Next(-15, 15),
                ClosePrice = random.Next(100, 500),
                PriceChange = random.Next(-50, 50)
            };
        }

        private RiskScoreCard GenerateMockRiskScoreCard()
        {
            var random = new Random();
            var currentScore = (decimal)(random.NextDouble() * 10);
            var previousScore = currentScore + (decimal)((random.NextDouble() - 0.5) * 2);
            var scoreChange = currentScore - previousScore;

            var riskCategory = currentScore switch
            {
                < 3 => RiskCategory.Low,
                < 5 => RiskCategory.ModerateLow,
                < 7 => RiskCategory.Moderate,
                < 9 => RiskCategory.ModerateHigh,
                _ => RiskCategory.High
            };

            return new RiskScoreCard
            {
                Score = Math.Round(currentScore, 1),
                RiskCategory = riskCategory,
                PreviousScore = Math.Round(previousScore, 1),
                ScoreChange = Math.Round(scoreChange, 1),
                Description = "Portfolio risk assessment based on asset allocation and volatility",
                LastCalculated = DateTime.Now,
                RiskFactors = GenerateMockRiskFactors(),
                Volatility = (decimal)(random.NextDouble() * 0.3),
                Beta = (decimal)(random.NextDouble() * 2),
                SharpeRatio = (decimal)(random.NextDouble() * 2),
                MaxDrawdown = (decimal)(random.NextDouble() * 0.2)
            };
        }

        private MonthlyDividendsCard GenerateMockMonthlyDividendsCard()
        {
            var random = new Random();
            var monthlyAmount = random.Next(200, 500);
            var previousMonthAmount = monthlyAmount - random.Next(-50, 50);
            var changeAmount = monthlyAmount - previousMonthAmount;
            var changePercentage = previousMonthAmount > 0 ? (changeAmount / previousMonthAmount) * 100 : 0;

            return new MonthlyDividendsCard
            {
                MonthlyAmount = monthlyAmount,
                PreviousMonthAmount = previousMonthAmount,
                ChangeAmount = changeAmount,
                ChangePercentage = Math.Round(changePercentage, 2),
                NextPaymentDate = DateTime.Now.AddDays(15),
                UpcomingPayments = GenerateMockUpcomingDividends(),
                RecentPayments = GenerateMockRecentDividends(),
                YearToDateTotal = monthlyAmount * 12,
                ProjectedAnnualIncome = monthlyAmount * 12,
                DividendYield = (decimal)(random.NextDouble() * 0.05),
                DividendPayingPositions = random.Next(5, 15),
                AverageDividendPerPosition = monthlyAmount / random.Next(5, 15)
            };
        }

        private AssetAllocationCard GenerateMockAssetAllocationCard(string breakdownType)
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
                        Color = GetRandomColor(),
                        TargetPercentage = 20m,
                        DeviationFromTarget = (value / totalValue * 100) - 20m
                    });
                    remainingValue -= value;
                }

                items.Add(new AssetAllocationItem
                {
                    Label = assetClasses[assetClasses.Length - 1],
                    Value = remainingValue,
                    Percentage = remainingValue / totalValue * 100,
                    Color = GetRandomColor(),
                    TargetPercentage = 20m,
                    DeviationFromTarget = (remainingValue / totalValue * 100) - 20m
                });
            }

            return new AssetAllocationCard
            {
                BreakdownType = breakdownType,
                Items = items,
                TotalPortfolioValue = items.Sum(i => i.Value),
                DiversificationScore = CalculateDiversificationScore(items),
                RiskAssessment = "Well diversified across asset classes",
                RebalancingRecommendations = GenerateMockRebalancingRecommendations(),
                LastRebalancingDate = DateTime.Now.AddDays(-30),
                TargetAllocations = items.ToDictionary(i => i.Label, i => i.TargetPercentage),
                AllocationDeviations = items.ToDictionary(i => i.Label, i => i.DeviationFromTarget)
            };
        }

        #endregion

        #region Helper Methods

        private List<PortfolioHolding> GenerateMockTopHoldings(int count)
        {
            var holdings = new List<PortfolioHolding>();
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

        private List<PortfolioDataPoint> GenerateMockPerformanceHistory(int months)
        {
            var data = new List<PortfolioDataPoint>();
            var random = new Random();
            var baseValue = 100000m;
            var currentDate = DateTime.Now.AddMonths(-months);

            for (int i = 0; i <= months; i++)
            {
                var volatility = (decimal)(random.NextDouble() - 0.5) * 0.1m;
                var growth = 0.02m;
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

        private MarketSessionStatus GetRandomMarketStatus()
        {
            var random = new Random();
            var values = Enum.GetValues<MarketSessionStatus>();
            return values[random.Next(values.Length)];
        }

        private List<RiskFactor> GenerateMockRiskFactors()
        {
            return new List<RiskFactor>
            {
                new RiskFactor { Name = "Market Volatility", Weight = 0.3m, Value = 4.2m, Description = "Overall market risk exposure" },
                new RiskFactor { Name = "Sector Concentration", Weight = 0.25m, Value = 3.8m, Description = "Diversification across sectors" },
                new RiskFactor { Name = "Geographic Exposure", Weight = 0.2m, Value = 2.5m, Description = "International market exposure" },
                new RiskFactor { Name = "Liquidity", Weight = 0.15m, Value = 1.8m, Description = "Ease of converting to cash" },
                new RiskFactor { Name = "Currency Risk", Weight = 0.1m, Value = 1.2m, Description = "Foreign exchange exposure" }
            };
        }

        private List<DividendPayment> GenerateMockUpcomingDividends()
        {
            return new List<DividendPayment>
            {
                new DividendPayment { Ticker = "AAPL", Company = "Apple Inc.", Amount = 87.50m, PaymentDate = DateTime.Now.AddDays(15), Status = DividendPaymentStatus.Upcoming, DividendPerShare = 0.25m, Shares = 350, ExDividendDate = DateTime.Now.AddDays(8) },
                new DividendPayment { Ticker = "MSFT", Company = "Microsoft Corp.", Amount = 76.00m, PaymentDate = DateTime.Now.AddDays(22), Status = DividendPaymentStatus.Upcoming, DividendPerShare = 0.75m, Shares = 101, ExDividendDate = DateTime.Now.AddDays(15) },
                new DividendPayment { Ticker = "JNJ", Company = "Johnson & Johnson", Amount = 45.68m, PaymentDate = DateTime.Now.AddDays(30), Status = DividendPaymentStatus.Upcoming, DividendPerShare = 1.19m, Shares = 38, ExDividendDate = DateTime.Now.AddDays(23) }
            };
        }

        private List<DividendPayment> GenerateMockRecentDividends()
        {
            return new List<DividendPayment>
            {
                new DividendPayment { Ticker = "V", Company = "Visa Inc.", Amount = 32.00m, PaymentDate = DateTime.Now.AddDays(-5), Status = DividendPaymentStatus.Paid, DividendPerShare = 0.20m, Shares = 160, ExDividendDate = DateTime.Now.AddDays(-18) },
                new DividendPayment { Ticker = "PG", Company = "Procter & Gamble Co.", Amount = 45.50m, PaymentDate = DateTime.Now.AddDays(-12), Status = DividendPaymentStatus.Paid, DividendPerShare = 0.94m, Shares = 48, ExDividendDate = DateTime.Now.AddDays(-25) },
                new DividendPayment { Ticker = "KO", Company = "Coca-Cola Co.", Amount = 28.00m, PaymentDate = DateTime.Now.AddDays(-18), Status = DividendPaymentStatus.Paid, DividendPerShare = 0.46m, Shares = 61, ExDividendDate = DateTime.Now.AddDays(-31) }
            };
        }

        private List<RebalancingRecommendation> GenerateMockRebalancingRecommendations()
        {
            return new List<RebalancingRecommendation>
            {
                new RebalancingRecommendation
                {
                    Category = "Stocks",
                    Action = RebalancingAction.Sell,
                    AdjustmentAmount = 25000m,
                    Priority = RecommendationPriority.Medium,
                    Reason = "Over-allocated by 5%"
                },
                new RebalancingRecommendation
                {
                    Category = "Bonds",
                    Action = RebalancingAction.Buy,
                    AdjustmentAmount = 15000m,
                    Priority = RecommendationPriority.Low,
                    Reason = "Under-allocated by 3%"
                }
            };
        }

        private decimal CalculateDiversificationScore(List<AssetAllocationItem> items)
        {
            if (!items.Any()) return 0;

            var totalValue = items.Sum(i => i.Value);
            var percentages = items.Select(i => i.Percentage).ToList();
            
            // Calculate Herfindahl-Hirschman Index (HHI) for concentration
            var hhi = percentages.Sum(p => (p / 100) * (p / 100));
            
            // Convert to diversification score (0-100)
            // Lower HHI = more diversified = higher score
            var diversificationScore = Math.Max(0, 100 - (hhi * 100));
            
            return Math.Round(diversificationScore, 1);
        }

        private string GetRandomColor()
        {
            var random = new Random();
            var colors = new[] { "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", "#DDA0DD", "#98D8C8", "#F7DC6F" };
            return colors[random.Next(colors.Length)];
        }

        private async Task<List<PortfolioInsight>> GenerateInsightsAsync(string userId, DashboardCardsSummary summary)
        {
            var insights = new List<PortfolioInsight>();

            // Generate insights based on portfolio data
            if (summary.RiskScore.Score > 7)
            {
                insights.Add(new PortfolioInsight
                {
                    Title = "High Risk Portfolio Detected",
                    Description = "Your portfolio risk score is elevated. Consider diversifying into more conservative assets.",
                    Category = InsightCategory.RiskManagement,
                    Priority = InsightPriority.High,
                    GeneratedAt = DateTime.Now,
                    RelatedTickers = new List<string>(),
                    ActionItems = new List<string> { "Review asset allocation", "Consider bond ETFs", "Reduce high-volatility positions" }
                });
            }

            if (summary.AssetAllocation.DisplayValues.NeedsRebalancing)
            {
                insights.Add(new PortfolioInsight
                {
                    Title = "Portfolio Rebalancing Recommended",
                    Description = "Your asset allocation has drifted from target percentages. Rebalancing can help maintain your desired risk profile.",
                    Category = InsightCategory.Rebalancing,
                    Priority = InsightPriority.Medium,
                    GeneratedAt = DateTime.Now,
                    RelatedTickers = new List<string>(),
                    ActionItems = new List<string> { "Review current allocations", "Execute rebalancing trades", "Set calendar reminder for quarterly review" }
                });
            }

            return insights;
        }

        private async Task<List<PortfolioAlert>> GenerateAlertsAsync(string userId, DashboardCardsSummary summary)
        {
            var alerts = new List<PortfolioAlert>();

            // Generate alerts based on portfolio data
            if (summary.TodaysPnl.TodayPnl < -5000)
            {
                alerts.Add(new PortfolioAlert
                {
                    Title = "Significant Daily Loss",
                    Message = "Your portfolio experienced a substantial loss today. Review your positions and consider risk management strategies.",
                    Severity = AlertSeverity.High,
                    Category = AlertCategory.Performance,
                    GeneratedAt = DateTime.Now,
                    IsAcknowledged = false,
                    RelatedTickers = new List<string>()
                });
            }

            if (summary.RiskScore.Score > 8)
            {
                alerts.Add(new PortfolioAlert
                {
                    Title = "Critical Risk Level",
                    Message = "Your portfolio risk score has reached a critical level. Immediate attention is recommended.",
                    Severity = AlertSeverity.Critical,
                    Category = AlertCategory.Risk,
                    GeneratedAt = DateTime.Now,
                    IsAcknowledged = false,
                    RelatedTickers = new List<string>()
                });
            }

            return alerts;
        }

        #endregion
    }
}
