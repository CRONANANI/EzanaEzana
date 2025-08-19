using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ezana.Models.PublicOfficials
{
    public class HouseTradingCard
    {
        // Core Data Properties
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ActiveTraders { get; set; }
        public int UniqueCompanies { get; set; }

        // Trading Metrics
        public decimal AverageTradeSize { get; set; }
        public decimal LargestTrade { get; set; }
        public decimal SmallestTrade { get; set; }
        public decimal TotalMarketValue { get; set; }

        // Time-based Analysis
        public List<MonthlyHouseTradingData> MonthlyData { get; set; } = new List<MonthlyHouseTradingData>();
        public List<YearlyHouseTradingData> YearlyData { get; set; } = new List<YearlyHouseTradingData>();
        public List<QuarterlyHouseTradingData> QuarterlyData { get; set; } = new List<QuarterlyHouseTradingData>();

        // Top Performers
        public List<TopHouseTrader> TopTraders { get; set; } = new List<TopHouseTrader>();
        public List<TopHouseTradedCompany> TopCompanies { get; set; } = new List<TopHouseTradedCompany>();
        public List<TopHouseSector> TopSectors { get; set; } = new List<TopHouseSector>();

        // Recent Activity
        public List<RecentHouseTrade> RecentTrades { get; set; } = new List<RecentHouseTrade>();
        public List<NotableHouseTrade> NotableTrades { get; set; } = new List<NotableHouseTrade>();

        // Party and Committee Analysis
        public List<PartyTradingData> PartyData { get; set; } = new List<PartyTradingData>();
        public List<CommitteeTradingData> CommitteeData { get; set; } = new List<CommitteeTradingData>();
        public List<StateHouseTradingData> StateData { get; set; } = new List<StateHouseTradingData>();

        // Performance and Risk
        public decimal AverageReturn { get; set; }
        public decimal BestPerformingTrader { get; set; }
        public decimal WorstPerformingTrader { get; set; }
        public int PotentialConflicts { get; set; }
        public List<HouseConflictAlert> ConflictAlerts { get; set; } = new List<HouseConflictAlert>();

        // Display Properties
        public string DisplayTotalTrades => $"{TotalTrades:N0}";
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayAverageReturn => $"{AverageReturn:F2}%";
        public string DisplayBestPerformingTrader => $"{BestPerformingTrader:F2}%";
        public string DisplayWorstPerformingTrader => $"{WorstPerformingTrader:F2}%";

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

            CalculatePerformanceMetrics();
            CalculatePartyMetrics();
        }

        private void CalculatePerformanceMetrics()
        {
            if (TopTraders?.Any() == true)
            {
                var returns = TopTraders.Where(t => t.TotalReturn != 0).Select(t => t.TotalReturn);
                if (returns.Any())
                {
                    AverageReturn = returns.Average();
                    BestPerformingTrader = returns.Max();
                    WorstPerformingTrader = returns.Min();
                }
            }
        }

        private void CalculatePartyMetrics()
        {
            if (PartyData?.Any() == true)
            {
                foreach (var party in PartyData)
                {
                    party.CalculateMetrics();
                }
            }
        }

        public List<MonthlyHouseTradingData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalVolume)
                .Take(count)
                .ToList() ?? new List<MonthlyHouseTradingData>();
        }

        public List<TopHouseTrader> GetTopTradersByVolume(int count = 10)
        {
            return TopTraders?
                .OrderByDescending(t => t.TotalVolume)
                .Take(count)
                .ToList() ?? new List<TopHouseTrader>();
        }

        public List<TopHouseTrader> GetTopTradersByReturn(int count = 10)
        {
            return TopTraders?
                .OrderByDescending(t => t.TotalReturn)
                .Take(count)
                .ToList() ?? new List<TopHouseTrader>();
        }

        public List<TopHouseTradedCompany> GetTopCompaniesByTrades(int count = 10)
        {
            return TopCompanies?
                .OrderByDescending(c => c.TotalTrades)
                .Take(count)
                .ToList() ?? new List<TopHouseTradedCompany>();
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
            
            if (growthRate > 12) return "Strongly Increasing";
            if (growthRate > 6) return "Increasing";
            if (growthRate > -6) return "Stable";
            if (growthRate > -12) return "Decreasing";
            return "Strongly Decreasing";
        }

        public List<PartyTradingData> GetPartyRanking()
        {
            return PartyData?
                .OrderByDescending(p => p.TotalVolume)
                .ToList() ?? new List<PartyTradingData>();
        }

        public List<CommitteeTradingData> GetTopCommittees(int count = 10)
        {
            return CommitteeData?
                .OrderByDescending(c => c.TotalVolume)
                .Take(count)
                .ToList() ?? new List<CommitteeTradingData>();
        }

        public List<StateHouseTradingData> GetTopStates(int count = 10)
        {
            return StateData?
                .OrderByDescending(s => s.TotalVolume)
                .Take(count)
                .ToList() ?? new List<StateHouseTradingData>();
        }

        public List<HouseConflictAlert> GetHighPriorityConflicts()
        {
            return ConflictAlerts?
                .Where(c => c.Priority == HouseConflictPriority.High)
                .OrderByDescending(c => c.Severity)
                .ToList() ?? new List<HouseConflictAlert>();
        }

        public decimal GetPartyPerformance(string party)
        {
            var partyData = PartyData?.FirstOrDefault(p => p.Party.Equals(party, StringComparison.OrdinalIgnoreCase));
            return partyData?.AverageReturn ?? 0;
        }

        public List<RecentHouseTrade> GetHighValueTrades(decimal threshold = 50000)
        {
            return RecentTrades?
                .Where(t => t.TradeValue >= threshold)
                .OrderByDescending(t => t.TradeValue)
                .ToList() ?? new List<RecentHouseTrade>();
        }
    }

    // Supporting Models
    public class MonthlyHouseTradingData
    {
        public DateTime Month { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public int ActiveTraders { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayVolume => $"{TotalVolume:C0}";
    }

    public class YearlyHouseTradingData
    {
        public int Year { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
    }

    public class QuarterlyHouseTradingData
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public string DisplayQuarter => $"Q{Quarter} {Year}";
        public string DisplayVolume => $"{TotalVolume:C0}";
    }

    public class TopHouseTrader
    {
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public List<string> Committees { get; set; } = new List<string>();
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public decimal TotalReturn { get; set; }
        public DateTime LastTradeDate { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayTotalReturn => $"{TotalReturn:F2}%";
        public string DisplayLastTradeDate => LastTradeDate.ToString("MMM dd, yyyy");
        public string DisplayCommittees => string.Join(", ", Committees);
    }

    public class TopHouseTradedCompany
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

    public class TopHouseSector
    {
        public string Sector { get; set; } = string.Empty;
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageVolume { get; set; }
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageVolume => $"{AverageVolume:C0}";
    }

    public class RecentHouseTrade
    {
        public DateTime TradeDate { get; set; }
        public string RepresentativeName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public HouseTradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public int Shares { get; set; }
        public decimal PricePerShare { get; set; }
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayPricePerShare => $"{PricePerShare:C2}";
        public string DisplayTradeType => Type.ToString();
    }

    public class NotableHouseTrade
    {
        public DateTime TradeDate { get; set; }
        public string RepresentativeName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public HouseTradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public string NotableReason { get; set; } = string.Empty;
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayTradeType => Type.ToString();
    }

    public class PartyTradingData
    {
        public string Party { get; set; } = string.Empty;
        public int TotalTraders { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public decimal AverageReturn { get; set; }
        public List<string> TopTraders { get; set; } = new List<string>();

        public void CalculateMetrics()
        {
            if (TotalTrades > 0)
            {
                AverageTradeSize = TotalVolume / TotalTrades;
            }
        }

        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayAverageReturn => $"{AverageReturn:F2}%";
    }

    public class CommitteeTradingData
    {
        public string CommitteeName { get; set; } = string.Empty;
        public string CommitteeType { get; set; } = string.Empty;
        public int TotalMembers { get; set; }
        public int ActiveTraders { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
    }

    public class StateHouseTradingData
    {
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int TotalRepresentatives { get; set; }
        public int ActiveTraders { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageVolume { get; set; }
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageVolume => $"{AverageVolume:C0}";
    }

    public class HouseConflictAlert
    {
        public string RepresentativeName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public HouseConflictPriority Priority { get; set; }
        public decimal Severity { get; set; }
        public DateTime AlertDate { get; set; }
        public string DisplayAlertDate => AlertDate.ToString("MMM dd, yyyy");
        public string DisplaySeverity => $"{Severity:F1}";
    }

    public enum HouseTradeType
    {
        Buy,
        Sell,
        Option,
        Short
    }

    public enum HouseConflictPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
