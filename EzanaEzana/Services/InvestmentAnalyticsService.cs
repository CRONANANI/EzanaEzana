using Ezana.Data;
using Ezana.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ezana.Services
{
    public class InvestmentAnalyticsService : IInvestmentAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public InvestmentAnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, object>> GetInvestmentRecommendations(string userId)
        {
            var preferences = await _context.InvestmentPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences == null)
            {
                // Default recommendations for users without preferences
                return new Dictionary<string, object>
                {
                    ["message"] = "Please set your investment preferences to get personalized recommendations.",
                    ["defaultRecommendations"] = new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            ["type"] = "ETF",
                            ["name"] = "Total Market Index Fund",
                            ["allocation"] = 60,
                            ["risk"] = "Medium"
                        },
                        new Dictionary<string, object>
                        {
                            ["type"] = "Bond",
                            ["name"] = "Total Bond Market Fund",
                            ["allocation"] = 30,
                            ["risk"] = "Low"
                        },
                        new Dictionary<string, object>
                        {
                            ["type"] = "Cash",
                            ["name"] = "High-Yield Savings",
                            ["allocation"] = 10,
                            ["risk"] = "Very Low"
                        }
                    }
                };
            }

            // Generate recommendations based on user preferences
            var recommendations = new List<Dictionary<string, object>>();

            // Stock recommendations based on risk tolerance
            if (preferences.StockAllocation > 0)
            {
                if (preferences.RiskTolerance >= 7)
                {
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Growth Stock ETF",
                        ["allocation"] = preferences.StockAllocation * 0.6m,
                        ["risk"] = "High"
                    });
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Small Cap ETF",
                        ["allocation"] = preferences.StockAllocation * 0.4m,
                        ["risk"] = "High"
                    });
                }
                else if (preferences.RiskTolerance >= 4)
                {
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Total Market ETF",
                        ["allocation"] = preferences.StockAllocation * 0.7m,
                        ["risk"] = "Medium"
                    });
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Dividend ETF",
                        ["allocation"] = preferences.StockAllocation * 0.3m,
                        ["risk"] = "Medium-Low"
                    });
                }
                else
                {
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Large Cap Value ETF",
                        ["allocation"] = preferences.StockAllocation * 0.8m,
                        ["risk"] = "Medium-Low"
                    });
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Stock",
                        ["name"] = "Dividend Aristocrats ETF",
                        ["allocation"] = preferences.StockAllocation * 0.2m,
                        ["risk"] = "Low"
                    });
                }
            }

            // Bond recommendations
            if (preferences.BondAllocation > 0)
            {
                recommendations.Add(new Dictionary<string, object>
                {
                    ["type"] = "Bond",
                    ["name"] = "Total Bond Market ETF",
                    ["allocation"] = preferences.BondAllocation,
                    ["risk"] = "Low"
                });
            }

            // Cash recommendations
            if (preferences.CashAllocation > 0)
            {
                recommendations.Add(new Dictionary<string, object>
                {
                    ["type"] = "Cash",
                    ["name"] = "High-Yield Savings Account",
                    ["allocation"] = preferences.CashAllocation,
                    ["risk"] = "Very Low"
                });
            }

            // Alternative investments
            if (preferences.AlternativeAllocation > 0)
            {
                if (preferences.ESGFocus)
                {
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Alternative",
                        ["name"] = "ESG ETF",
                        ["allocation"] = preferences.AlternativeAllocation,
                        ["risk"] = "Medium"
                    });
                }
                else
                {
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Alternative",
                        ["name"] = "REIT ETF",
                        ["allocation"] = preferences.AlternativeAllocation * 0.5m,
                        ["risk"] = "Medium-High"
                    });
                    recommendations.Add(new Dictionary<string, object>
                    {
                        ["type"] = "Alternative",
                        ["name"] = "Commodity ETF",
                        ["allocation"] = preferences.AlternativeAllocation * 0.5m,
                        ["risk"] = "High"
                    });
                }
            }

            return new Dictionary<string, object>
            {
                ["message"] = "Based on your investment preferences, here are your personalized recommendations:",
                ["recommendations"] = recommendations
            };
        }

        public async Task<Dictionary<string, object>> GetPortfolioAllocation(string userId)
        {
            var preferences = await _context.InvestmentPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences == null)
            {
                return new Dictionary<string, object>
                {
                    ["message"] = "Please set your investment preferences to see your portfolio allocation.",
                    ["allocation"] = new Dictionary<string, decimal>
                    {
                        ["Stocks"] = 0,
                        ["Bonds"] = 0,
                        ["Cash"] = 0,
                        ["Alternative"] = 0
                    }
                };
            }

            var allocation = new Dictionary<string, decimal>
            {
                ["Stocks"] = preferences.StockAllocation,
                ["Bonds"] = preferences.BondAllocation,
                ["Cash"] = preferences.CashAllocation,
                ["Alternative"] = preferences.AlternativeAllocation
            };

            return new Dictionary<string, object>
            {
                ["message"] = "Your current portfolio allocation:",
                ["allocation"] = allocation
            };
        }
    }
} 