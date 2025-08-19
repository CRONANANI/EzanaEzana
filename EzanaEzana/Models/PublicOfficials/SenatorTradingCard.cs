using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EzanaEzana.Models.PublicOfficials
{
    public class SenatorTradingCard
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
        public List<MonthlySenatorTradingData> MonthlyData { get; set; } = new List<MonthlySenatorTradingData>();
        public List<YearlySenatorTradingData> YearlyData { get; set; } = new List<YearlySenatorTradingData>();
        public List<QuarterlySenatorTradingData> QuarterlyData { get; set; } = new List<QuarterlySenatorTradingData>();

        // Top Performers
        public List<TopSenatorTrader> TopTraders { get; set; } = new List<TopSenatorTrader>();
        public List<TopSenatorTradedCompany> TopCompanies { get; set; } = new List<TopSenatorTradedCompany>();
        public List<TopSenatorSector> TopSectors { get; set; } = new List<TopSenatorSector>();

        // Recent Activity
        public List<RecentSenatorTrade> RecentTrades { get; set; } = new List<RecentSenatorTrade>();
        public List<NotableSenatorTrade> NotableTrades { get; set; } = new List<NotableSenatorTrade>();

        // Party and Committee Analysis
        public List<SenatorPartyTradingData> PartyData { get; set; } = new List<SenatorPartyTradingData>();
        public List<SenatorCommitteeTradingData> CommitteeData { get; set; } = new List<SenatorCommitteeTradingData>();
        public List<StateSenatorTradingData> StateData { get; set; } = new List<StateSenatorTradingData>();

        // Performance and Risk
        public decimal AverageReturn { get; set; }
        public decimal BestPerformingTrader { get; set; }
        public decimal WorstPerformingTrader { get; set; }
        public int PotentialConflicts { get; set; }
        public List<SenatorConflictAlert> ConflictAlerts { get; set; } = new List<SenatorConflictAlert>();

        // Term and Re-election Analysis
        public List<TermTradingData> TermData { get; set; } = new List<TermTradingData>();
        public List<ReelectionTradingData> ReelectionData { get; set; } = new List<ReelectionTradingData>();

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
            CalculateTermMetrics();
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

        private void CalculateTermMetrics()
        {
            if (TermData?.Any() == true)
            {
                foreach (var term in TermData)
                {
                    term.CalculateMetrics();
                }
            }
        }

        public List<MonthlySenatorTradingData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalVolume)
                .Take(count)
                .ToList() ?? new List<MonthlySenatorTradingData>();
        }

        public List<TopSenatorTrader> GetTopTradersByVolume(int count = 10)
        {
            return TopTraders?
                .OrderByDescending(t => t.TotalVolume)
                .Take(count)
                .ToList() ?? new List<TopSenatorTrader>();
        }

        public List<TopSenatorTrader> GetTopTradersByReturn(int count = 10)
        {
            return TopTraders?
                .OrderByDescending(t => t.TotalReturn)
                .Take(count)
                .ToList() ?? new List<TopSenatorTrader>();
        }

        public List<TopSenatorTradedCompany> GetTopCompaniesByTrades(int count = 10)
        {
            return TopCompanies?
                .OrderByDescending(c => c.TotalTrades)
                .Take(count)
                .ToList() ?? new List<TopSenatorTradedCompany>();
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
            
            if (growthRate > 15) return "Strongly Increasing";
            if (growthRate > 8) return "Increasing";
            if (growthRate > -8) return "Stable";
            if (growthRate > -15) return "Decreasing";
            return "Strongly Decreasing";
        }

        public List<SenatorPartyTradingData> GetPartyRanking()
        {
            return PartyData?
                .OrderByDescending(p => p.TotalVolume)
                .ToList() ?? new List<SenatorPartyTradingData>();
        }

        public List<SenatorCommitteeTradingData> GetTopCommittees(int count = 10)
        {
            return CommitteeData?
                .OrderByDescending(c => c.TotalVolume)
                .Take(count)
                .ToList() ?? new List<SenatorCommitteeTradingData>();
        }

        public List<StateSenatorTradingData> GetTopStates(int count = 10)
        {
            return StateData?
                .OrderByDescending(s => s.TotalVolume)
                .Take(count)
                .ToList() ?? new List<StateSenatorTradingData>();
        }

        public List<SenatorConflictAlert> GetHighPriorityConflicts()
        {
            return ConflictAlerts?
                .Where(c => c.Priority == SenatorConflictPriority.High)
                .OrderByDescending(c => c.Severity)
                .ToList() ?? new List<SenatorConflictAlert>();
        }

        public decimal GetPartyPerformance(string party)
        {
            var partyData = PartyData?.FirstOrDefault(p => p.Party.Equals(party, StringComparison.OrdinalIgnoreCase));
            return partyData?.AverageReturn ?? 0;
        }

        public List<RecentSenatorTrade> GetHighValueTrades(decimal threshold = 100000)
        {
            return RecentTrades?
                .Where(t => t.TradeValue >= threshold)
                .OrderByDescending(t => t.TradeValue)
                .ToList() ?? new List<RecentSenatorTrade>();
        }

        public List<TermTradingData> GetTermPerformance()
        {
            return TermData?
                .OrderByDescending(t => t.TermStart)
                .ToList() ?? new List<TermTradingData>();
        }

        public List<ReelectionTradingData> GetReelectionTrading()
        {
            return ReelectionData?
                .OrderByDescending(r => r.ElectionYear)
                .ToList() ?? new List<ReelectionTradingData>();
        }
    }

    // Supporting Models
    public class MonthlySenatorTradingData
    {
        public DateTime Month { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public int ActiveTraders { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayVolume => $"{TotalVolume:C0}";
    }

    public class YearlySenatorTradingData
    {
        public int Year { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public string DisplayVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
    }

    public class QuarterlySenatorTradingData
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public string DisplayQuarter => $"Q{Quarter} {Year}";
        public string DisplayVolume => $"{TotalVolume:C0}";
    }

    public class TopSenatorTrader
    {
        public string Name { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
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
        public string DisplayTerm => $"{TermStart:yyyy} - {TermEnd:yyyy}";
    }

    public class TopSenatorTradedCompany
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

    public class TopSenatorSector
    {
        public string Sector { get; set; } = string.Empty;
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageVolume { get; set; }
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageVolume => $"{AverageVolume:C0}";
    }

    public class RecentSenatorTrade
    {
        public DateTime TradeDate { get; set; }
        public string SenatorName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public SenatorTradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public int Shares { get; set; }
        public decimal PricePerShare { get; set; }
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayPricePerShare => $"{PricePerShare:C2}";
        public string DisplayTradeType => Type.ToString();
    }

    public class NotableSenatorTrade
    {
        public DateTime TradeDate { get; set; }
        public string SenatorName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public SenatorTradeType Type { get; set; }
        public decimal TradeValue { get; set; }
        public string NotableReason { get; set; } = string.Empty;
        public string DisplayTradeDate => TradeDate.ToString("MMM dd, yyyy");
        public string DisplayTradeValue => $"{TradeValue:C0}";
        public string DisplayTradeType => Type.ToString();
    }

    public class SenatorPartyTradingData
    {
        public string Party { get; set; } = string.Empty;
        public int TotalSenators { get; set; }
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

    public class SenatorCommitteeTradingData
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

    public class StateSenatorTradingData
    {
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int TotalSenators { get; set; }
        public int ActiveTraders { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageVolume { get; set; }
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageVolume => $"{AverageVolume:C0}";
    }

    public class TermTradingData
    {
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public decimal TotalReturn { get; set; }

        public void CalculateMetrics()
        {
            if (TotalTrades > 0)
            {
                AverageTradeSize = TotalVolume / TotalTrades;
            }
        }

        public string DisplayTerm => $"{TermStart:yyyy} - {TermEnd:yyyy}";
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayTotalReturn => $"{TotalReturn:F2}%";
    }

    public class ReelectionTradingData
    {
        public int ElectionYear { get; set; }
        public string SenatorName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public bool WonReelection { get; set; }
        public int TotalTrades { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageTradeSize { get; set; }
        public string DisplayElectionYear => ElectionYear.ToString();
        public string DisplayTotalVolume => $"{TotalVolume:C0}";
        public string DisplayAverageTradeSize => $"{AverageTradeSize:C0}";
        public string DisplayReelectionResult => WonReelection ? "Won" : "Lost";
    }

    public class SenatorConflictAlert
    {
        public string SenatorName { get; set; } = string.Empty;
        public string Party { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SenatorConflictPriority Priority { get; set; }
        public decimal Severity { get; set; }
        public DateTime AlertDate { get; set; }
        public string DisplayAlertDate => AlertDate.ToString("MMM dd, yyyy");
        public string DisplaySeverity => $"{Severity:F1}";
    }

    public enum SenatorTradeType
    {
        Buy,
        Sell,
        Option,
        Short
    }

    public enum SenatorConflictPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
