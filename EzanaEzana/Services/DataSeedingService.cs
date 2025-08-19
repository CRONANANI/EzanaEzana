using EzanaEzana.Data;
using EzanaEzana.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzanaEzana.Services
{
    public class DataSeedingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataSeedingService> _logger;

        public DataSeedingService(ApplicationDbContext context, ILogger<DataSeedingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                _logger.LogInformation("Starting data seeding process...");

                await SeedAchievementsAsync();
                await SeedSampleCompaniesAsync();
                await SeedSampleMarketDataAsync();
                await SeedSampleEconomicIndicatorsAsync();
                await SeedSampleCommunityCategoriesAsync();

                _logger.LogInformation("Data seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data seeding");
                throw;
            }
        }

        private async Task SeedAchievementsAsync()
        {
            if (await _context.Achievements.AnyAsync())
            {
                _logger.LogInformation("Achievements already exist, skipping seeding.");
                return;
            }

            var achievements = new List<Achievement>
            {
                // Investment Achievements
                new Achievement
                {
                    Name = "First Stock",
                    Description = "Purchase your first stock",
                    Category = "Investment",
                    Type = "FirstStock",
                    IconClass = "fas fa-chart-line",
                    ColorScheme = "success",
                    Points = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Stock Diversifier",
                    Description = "Hold stocks in 5 different companies",
                    Category = "Investment",
                    Type = "StockDiversifier",
                    IconClass = "fas fa-layer-group",
                    ColorScheme = "info",
                    Points = 25,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Portfolio Builder",
                    Description = "Hold stocks in 10 different companies",
                    Category = "Investment",
                    Type = "PortfolioBuilder",
                    IconClass = "fas fa-building",
                    ColorScheme = "primary",
                    Points = 50,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Value Investor",
                    Description = "Build a portfolio worth $10,000",
                    Category = "Investment",
                    Type = "ValueInvestor",
                    IconClass = "fas fa-dollar-sign",
                    ColorScheme = "warning",
                    Points = 100,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Millionaire",
                    Description = "Build a portfolio worth $1,000,000",
                    Category = "Investment",
                    Type = "Millionaire",
                    IconClass = "fas fa-crown",
                    ColorScheme = "danger",
                    Points = 1000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // Community Achievements
                new Achievement
                {
                    Name = "First Post",
                    Description = "Create your first community thread",
                    Category = "Community",
                    Type = "FirstPost",
                    IconClass = "fas fa-comment",
                    ColorScheme = "success",
                    Points = 15,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Active Poster",
                    Description = "Create 10 community threads",
                    Category = "Community",
                    Type = "ActivePoster",
                    IconClass = "fas fa-comments",
                    ColorScheme = "info",
                    Points = 40,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Popular Poster",
                    Description = "Receive 50 total likes on your threads",
                    Category = "Community",
                    Type = "PopularPoster",
                    IconClass = "fas fa-heart",
                    ColorScheme = "danger",
                    Points = 75,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "First Comment",
                    Description = "Post your first comment",
                    Category = "Community",
                    Type = "FirstComment",
                    IconClass = "fas fa-reply",
                    ColorScheme = "success",
                    Points = 10,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Helpful Commenter",
                    Description = "Receive 25 total likes on your comments",
                    Category = "Community",
                    Type = "HelpfulCommenter",
                    IconClass = "fas fa-thumbs-up",
                    ColorScheme = "info",
                    Points = 50,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Community Leader",
                    Description = "Reach 1,000 total thread views",
                    Category = "Community",
                    Type = "CommunityLeader",
                    IconClass = "fas fa-star",
                    ColorScheme = "warning",
                    Points = 100,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                // Learning Achievements
                new Achievement
                {
                    Name = "First Research",
                    Description = "Complete your first market research",
                    Category = "Learning",
                    Type = "FirstResearch",
                    IconClass = "fas fa-search",
                    ColorScheme = "success",
                    Points = 20,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Research Enthusiast",
                    Description = "Complete 20 market research activities",
                    Category = "Learning",
                    Type = "ResearchEnthusiast",
                    IconClass = "fas fa-microscope",
                    ColorScheme = "info",
                    Points = 60,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Market Analyst",
                    Description = "Complete 50 market research activities",
                    Category = "Learning",
                    Type = "MarketAnalyst",
                    IconClass = "fas fa-chart-bar",
                    ColorScheme = "primary",
                    Points = 150,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Achievement
                {
                    Name = "Weekly Learner",
                    Description = "Complete research activities in 7 consecutive days",
                    Category = "Learning",
                    Type = "WeeklyLearner",
                    IconClass = "fas fa-calendar-check",
                    ColorScheme = "success",
                    Points = 30,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.Achievements.AddRange(achievements);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} achievements", achievements.Count);
        }

        private async Task SeedSampleCompaniesAsync()
        {
            if (await _context.CompanyProfiles.AnyAsync())
            {
                _logger.LogInformation("Company profiles already exist, skipping seeding.");
                return;
            }

            var companies = new List<CompanyProfile>
            {
                new CompanyProfile
                {
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Sector = "Technology",
                    Industry = "Consumer Electronics",
                    MarketCap = 3000000000000,
                    PE_Ratio = 25.5m,
                    DividendYield = 0.5m,
                    Beta = 1.2m,
                    Description = "Apple Inc. designs, manufactures, and markets smartphones, personal computers, tablets, wearables, and accessories worldwide.",
                    Website = "https://www.apple.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Sector = "Technology",
                    Industry = "Software",
                    MarketCap = 2800000000000,
                    PE_Ratio = 30.2m,
                    DividendYield = 0.8m,
                    Beta = 0.9m,
                    Description = "Microsoft Corporation develops, licenses, and supports software, services, devices, and solutions worldwide.",
                    Website = "https://www.microsoft.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Symbol = "GOOGL",
                    CompanyName = "Alphabet Inc.",
                    Sector = "Technology",
                    Industry = "Internet Services",
                    MarketCap = 1800000000000,
                    PE_Ratio = 28.1m,
                    DividendYield = 0.0m,
                    Beta = 1.1m,
                    Description = "Alphabet Inc. provides online advertising services in the United States, Europe, the Middle East, Africa, the Asia-Pacific, Canada, and Latin America.",
                    Website = "https://www.google.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Symbol = "TSLA",
                    CompanyName = "Tesla, Inc.",
                    Sector = "Consumer Discretionary",
                    Industry = "Automobiles",
                    MarketCap = 800000000000,
                    PE_Ratio = 75.3m,
                    DividendYield = 0.0m,
                    Beta = 2.1m,
                    Description = "Tesla, Inc. designs, develops, manufactures, leases, and sells electric vehicles, and energy generation and storage systems.",
                    Website = "https://www.tesla.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CompanyProfile
                {
                    Symbol = "JPM",
                    CompanyName = "JPMorgan Chase & Co.",
                    Sector = "Financial Services",
                    Industry = "Banks",
                    MarketCap = 450000000000,
                    PE_Ratio = 12.8m,
                    DividendYield = 2.5m,
                    Beta = 1.0m,
                    Description = "JPMorgan Chase & Co. operates as a financial services company worldwide.",
                    Website = "https://www.jpmorganchase.com",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.CompanyProfiles.AddRange(companies);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} company profiles", companies.Count);
        }

        private async Task SeedSampleMarketDataAsync()
        {
            if (await _context.MarketData.AnyAsync())
            {
                _logger.LogInformation("Market data already exists, skipping seeding.");
                return;
            }

            var marketData = new List<MarketData>
            {
                new MarketData
                {
                    Symbol = "AAPL",
                    CurrentPrice = 175.50m,
                    OpenPrice = 174.20m,
                    HighPrice = 176.80m,
                    LowPrice = 173.90m,
                    PriceChange = 1.30m,
                    PriceChangePercent = 0.75m,
                    Volume = 45000000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new MarketData
                {
                    Symbol = "MSFT",
                    CurrentPrice = 380.25m,
                    OpenPrice = 378.90m,
                    HighPrice = 382.10m,
                    LowPrice = 377.50m,
                    PriceChange = 1.35m,
                    PriceChangePercent = 0.36m,
                    Volume = 28000000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new MarketData
                {
                    Symbol = "GOOGL",
                    CurrentPrice = 140.80m,
                    OpenPrice = 139.50m,
                    HighPrice = 141.20m,
                    LowPrice = 138.90m,
                    PriceChange = 1.30m,
                    PriceChangePercent = 0.93m,
                    Volume = 35000000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new MarketData
                {
                    Symbol = "TSLA",
                    CurrentPrice = 245.60m,
                    OpenPrice = 242.30m,
                    HighPrice = 248.90m,
                    LowPrice = 241.10m,
                    PriceChange = 3.30m,
                    PriceChangePercent = 1.36m,
                    Volume = 65000000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                },
                new MarketData
                {
                    Symbol = "JPM",
                    CurrentPrice = 165.40m,
                    OpenPrice = 164.80m,
                    HighPrice = 166.20m,
                    LowPrice = 164.20m,
                    PriceChange = 0.60m,
                    PriceChangePercent = 0.36m,
                    Volume = 18000000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                }
            };

            _context.MarketData.AddRange(marketData);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} market data entries", marketData.Count);
        }

        private async Task SeedSampleEconomicIndicatorsAsync()
        {
            if (await _context.EconomicIndicators.AnyAsync())
            {
                _logger.LogInformation("Economic indicators already exist, skipping seeding.");
                return;
            }

            var indicators = new List<EconomicIndicator>
            {
                new EconomicIndicator
                {
                    Name = "GDP Growth Rate",
                    Category = "Economic Growth",
                    Description = "Annual percentage change in Gross Domestic Product",
                    Unit = "Percentage",
                    Frequency = "Quarterly",
                    Source = "Bureau of Economic Analysis",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new EconomicIndicator
                {
                    Name = "Unemployment Rate",
                    Category = "Labor Market",
                    Description = "Percentage of labor force that is unemployed",
                    Unit = "Percentage",
                    Frequency = "Monthly",
                    Source = "Bureau of Labor Statistics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new EconomicIndicator
                {
                    Name = "Inflation Rate (CPI)",
                    Category = "Price Stability",
                    Description = "Annual percentage change in Consumer Price Index",
                    Unit = "Percentage",
                    Frequency = "Monthly",
                    Source = "Bureau of Labor Statistics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new EconomicIndicator
                {
                    Name = "Federal Funds Rate",
                    Category = "Monetary Policy",
                    Description = "Target interest rate set by Federal Reserve",
                    Unit = "Percentage",
                    Frequency = "As Needed",
                    Source = "Federal Reserve",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new EconomicIndicator
                {
                    Name = "Consumer Confidence Index",
                    Category = "Consumer Sentiment",
                    Description = "Measure of consumer optimism about the economy",
                    Unit = "Index",
                    Frequency = "Monthly",
                    Source = "Conference Board",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.EconomicIndicators.AddRange(indicators);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} economic indicators", indicators.Count);
        }

        private async Task SeedSampleCommunityCategoriesAsync()
        {
            if (await _context.CommunityCategories.AnyAsync())
            {
                _logger.LogInformation("Community categories already exist, skipping seeding.");
                return;
            }

            var categories = new List<CommunityCategory>
            {
                new CommunityCategory
                {
                    Name = "General Discussion",
                    Description = "General topics and discussions about investing",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CommunityCategory
                {
                    Name = "Stock Analysis",
                    Description = "Technical and fundamental analysis of stocks",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CommunityCategory
                {
                    Name = "Portfolio Management",
                    Description = "Portfolio strategies and management techniques",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CommunityCategory
                {
                    Name = "Market News",
                    Description = "Discussion of market news and events",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CommunityCategory
                {
                    Name = "Investment Strategies",
                    Description = "Long-term investment strategies and philosophies",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new CommunityCategory
                {
                    Name = "Risk Management",
                    Description = "Risk management techniques and discussions",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.CommunityCategories.AddRange(categories);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} community categories", categories.Count);
        }
    }
}
