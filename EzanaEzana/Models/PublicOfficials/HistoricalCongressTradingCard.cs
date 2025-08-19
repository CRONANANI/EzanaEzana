using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EzanaEzana.Models.PublicOfficials
{
    public class HistoricalCongressTradingCard
    {
        // Core Data Properties
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ActiveTraders { get; set; }
        public int UniqueCompanies { get; set; }

        // Performance Metrics
        public decimal AverageTradeSize { get; set; }
        public decimal LargestTrade { get; set; }
        public decimal SmallestTrade { get; set; }
        public decimal TotalMarketValue { get; set; }

        // Time-based Analysis
        public List<MonthlyTradingData> MonthlyData { get; set; } = new List<MonthlyTradingData>();
        public List<YearlyTradingData> YearlyData { get; set; } = new List<YearlyTradingData>();

        // Top Performers
        public List<TopCongressTrader> TopTraders { get; set; } = new List<TopCongressTrader>();
        public List<TopTradedCompany> TopCompanies { get; set; } = new List<TopTradedCompany>();

        // Recent Activity
        public List<RecentTrade> RecentTrades { get; set; } = new List<RecentTrade>();
        public List<NotableTrade> NotableTrades { get; set; } = new List<NotableTrade>();

        // Risk and Compliance
        public int PotentialConflicts { get; set; }
        public List<ConflictAlert> ConflictAlerts { get; set; } = new List<ConflictAlert>();
        public decimal ComplianceScore { get; set; }

        // Display Properties
        public string DisplayTotalTrades => $"{TotalTrades:N0}";
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayComplianceScore => $"{ComplianceScore:F1}%";

        // Calculation Methods
        public void CalculateMetrics()
        {
            if (RecentTrades?.Any() == true)
            {
                AverageTradeSize = RecentTrades.Average(t => t.TradeValue);
                LargestTrade = RecentTrades.Max(t => t.TradeValue);
                SmallestTrade = RecentTrades.Min(t => t.TradeValue);
                TotalMarketValue = RecentTrades.Sum(t => t.TradeValue);
            }

            if (MonthlyData?.Any() == true)
            {
                var currentMonth = MonthlyData.OrderByDescending(m => m.Month).FirstOrDefault();
                if (currentMonth != null)
                {
                    TotalTrades = currentMonth.TotalTrades;
                    TotalVolume = currentMonth.TotalVolume;
                }
            }

            CalculateComplianceScore();
        }

        private void CalculateComplianceScore()
        {
            if (TotalTrades == 0) return;

            var compliantTrades = TotalTrades - PotentialConflicts;
            ComplianceScore = (decimal)compliantTrades / TotalTrades * 100;
        }

        public List<MonthlyTradingData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalVolume)
                .Take(count)
                .ToList() ?? new List<MonthlyTradingData>();
        }

        public List<TopCongressTrader> GetTopTradersByVolume(int count = 10)
        {
            return TopTraders?
                .OrderByDescending(t => t.TotalVolume)
                .Take(count)
                .ToList() ?? new List<TopCongressTrader>();
        }

        public List<TopTradedCompany> GetTopCompaniesByTrades(int count = 10)
        {
            return TopCompanies?
                .OrderByDescending(c => c.TotalTrades)
                .Take(count)
                .ToList() ?? new List<TopTradedCompany>();
        }

        public decimal GetMonthlyGrowthRate()
        {
            if (MonthlyData?.Count < 2) return 0;

            var currentMonth = MonthlyData.OrderByDescending(m => m.Month).First();
            var previousMonth = MonthlyData.OrderByDescending(m => m.Month).Skip(1).First();

            if (previousMonth.TotalVolume == 0) return 0;

            return ((currentMonth.TotalVolume - previousMonth.TotalVolume) / previousMonth.TotalVolume) * 100;
        }

        public string GetTradingTrend()
        {
            var growthRate = GetMonthlyGrowthRate();
            
            if (growthRate > 10) return "Strongly Increasing";
            if (growthRate > 5) return "Increasing";
            if (growthRate > -5) return "Stable";
            if (growthRate > -10) return "Decreasing";
            return "Strongly Decreasing";
        }

        public List<ConflictAlert> GetHighPriorityConflicts()
        {
            return ConflictAlerts?
                .Where(c => c.Priority == ConflictPriority.High)
                .OrderByDescending(c => c.Severity)
                .ToList() ?? new List<ConflictAlert>();
        }
    }

    // Supporting Models
    public class MonthlyTradingData
    {
        public DateTime Month { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public int ActiveTraders { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayVolume => $"{TotalVolume:C0}";
    }

    public class YearlyTradingData
    {
        public int Year { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
    }

    public class TopCongressTrader
    {
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Chamber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public DateTime LastTradeDate { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayLastTradeDate => LastTradeDate.ToString("MMM dd, yyyy");
    }

    public class TopTradedCompany
    {
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public DateTime LastTradeDate { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayLastTradeDate => LastTradeDate.ToString("MMM dd, yyyy");
    }

    public class RecentTrade
    {
        public DateTime TradeDate { get; set; }
        public string CongresspersonName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public TradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public int Shares { get; set; }
        public decimal PricePerShare { get; set; }
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayPricePerShare => $"{PricePerShare:C2}";
        public string DisplayTradeType => Type.ToString();
    }

    public class NotableTrade
    {
        public DateTime TradeDate { get; set; }
        public string CongresspersonName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public TradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayTradeType => Type.ToString();
    }

    public class ConflictAlert
    {
        public string CongresspersonName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ConflictPriority Priority { get; set; }
        public decimal Severity { get; set; }
        public DateTime AlertDate { get; set; }
        public string DisplayAlertDate => AlertDate.ToString("MMM dd, yyyy");
        public string DisplaySeverity => $"{Severity:F1}";
    }

    public enum TradeType
    {
        Buy,
        Sell,
        Option,
        Short
    }

    public enum ConflictPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
