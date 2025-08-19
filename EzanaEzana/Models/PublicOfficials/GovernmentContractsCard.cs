using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EzanaEzana.Models.PublicOfficials
{
    public class GovernmentContractsCard
    {
        // Core Data Properties
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ActiveCompanies { get; set; }
        public int UniqueAgencies { get; set; }

        // Contract Metrics
        public decimal AverageContractValue { get; set; }
        public decimal LargestContract { get; set; }
        public decimal SmallestContract { get; set; }
        public decimal TotalAwardedValue { get; set; }

        // Time-based Analysis
        public List<MonthlyContractData> MonthlyData { get; set; } = new List<MonthlyContractData>();
        public List<YearlyContractData> YearlyData { get; set; } = new List<YearlyContractData>();
        public List<QuarterlyContractData> QuarterlyData { get; set; } = new List<QuarterlyContractData>();

        // Top Performers
        public List<TopContractor> TopContractors { get; set; } = new List<TopContractor>();
        public List<TopContractingAgency> TopAgencies { get; set; } = new List<TopContractingAgency>();
        public List<TopContractCategory> TopCategories { get; set; } = new List<TopContractCategory>();

        // Recent Activity
        public List<RecentContract> RecentContracts { get; set; } = new List<RecentContract>();
        public List<NotableContract> NotableContracts { get; set; } = new List<NotableContract>();

        // Geographic Distribution
        public List<StateContractData> StateData { get; set; } = new List<StateContractData>();
        public List<RegionContractData> RegionData { get; set; } = new List<RegionContractData>();

        // Contract Types and Categories
        public List<ContractTypeBreakdown> ContractTypes { get; set; } = new List<ContractTypeBreakdown>();
        public List<IndustryBreakdown> IndustryData { get; set; } = new List<IndustryBreakdown>();

        // Performance Metrics
        public decimal CompletionRate { get; set; }
        public decimal OnTimeDeliveryRate { get; set; }
        public decimal CostSavingsRate { get; set; }
        public decimal QualityRating { get; set; }

        // Display Properties
        public string DisplayTotalContracts => $"{TotalContracts:N0}";
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayAverageContractValue => $"{AverageContractValue:C0}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayCompletionRate => $"{CompletionRate:F1}%";
        public string DisplayOnTimeDeliveryRate => $"{OnTimeDeliveryRate:F1}%";
        public string DisplayCostSavingsRate => $"{CostSavingsRate:F1}%";
        public string DisplayQualityRating => $"{QualityRating:F1}";

        // Calculation Methods
        public void CalculateMetrics()
        {
            if (RecentContracts?.Any() == true)
            {
                AverageContractValue = RecentContracts.Average(c => c.ContractValue);
                LargestContract = RecentContracts.Max(c => c.ContractValue);
                SmallestContract = RecentContracts.Min(c => c.ContractValue);
                TotalAwardedValue = RecentContracts.Sum(c => c.ContractValue);
            }

            if (MonthlyData?.Any() == true)
            {
                var currentMonth = MonthlyData.OrderByDescending(m => m.Month).FirstOrDefault();
                if (currentMonth != null)
                {
                    TotalContracts = currentMonth.TotalContracts;
                    TotalValue = currentMonth.TotalValue;
                }
            }

            CalculatePerformanceMetrics();
        }

        private void CalculatePerformanceMetrics()
        {
            if (RecentContracts?.Any() == true)
            {
                var completedContracts = RecentContracts.Where(c => c.Status == ContractStatus.Completed);
                var onTimeContracts = RecentContracts.Where(c => c.Status == ContractStatus.Completed && c.IsOnTime);
                var costSavingsContracts = RecentContracts.Where(c => c.Status == ContractStatus.Completed && c.CostSavings > 0);

                if (completedContracts.Any())
                {
                    CompletionRate = (decimal)completedContracts.Count() / RecentContracts.Count * 100;
                    OnTimeDeliveryRate = (decimal)onTimeContracts.Count() / completedContracts.Count() * 100;
                    CostSavingsRate = (decimal)costSavingsContracts.Count() / completedContracts.Count * 100;
                    QualityRating = completedContracts.Average(c => c.QualityScore);
                }
            }
        }

        public List<MonthlyContractData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalValue)
                .Take(count)
                .ToList() ?? new List<MonthlyContractData>();
        }

        public List<TopContractor> GetTopContractorsByValue(int count = 10)
        {
            return TopContractors?
                .OrderByDescending(c => c.TotalContractValue)
                .Take(count)
                .ToList() ?? new List<TopContractor>();
        }

        public List<TopContractingAgency> GetTopAgenciesByValue(int count = 10)
        {
            return TopAgencies?
                .OrderByDescending(a => a.TotalContractValue)
                .Take(count)
                .ToList() ?? new List<TopContractingAgency>();
        }

        public decimal GetMonthlyGrowthRate()
        {
            if (MonthlyData?.Count < 2) return 0;

            var currentMonth = MonthlyData.OrderByDescending(m => m.Month).First();
            var previousMonth = MonthlyData.OrderByDescending(m => m.Month).Skip(1).First();

            if (previousMonth.TotalValue == 0) return 0;

            return ((currentMonth.TotalValue - previousMonth.TotalValue) / previousMonth.TotalValue) * 100;
        }

        public string GetContractTrend()
        {
            var growthRate = GetMonthlyGrowthRate();
            
            if (growthRate > 15) return "Strongly Increasing";
            if (growthRate > 8) return "Increasing";
            if (growthRate > -8) return "Stable";
            if (growthRate > -15) return "Decreasing";
            return "Strongly Decreasing";
        }

        public List<RecentContract> GetHighValueContracts(decimal threshold = 1000000)
        {
            return RecentContracts?
                .Where(c => c.ContractValue >= threshold)
                .OrderByDescending(c => c.ContractValue)
                .ToList() ?? new List<RecentContract>();
        }

        public List<StateContractData> GetTopStates(int count = 10)
        {
            return StateData?
                .OrderByDescending(s => s.TotalValue)
                .Take(count)
                .ToList() ?? new List<StateContractData>();
        }

        public decimal GetRegionalDistribution(string region)
        {
            var regionData = RegionData?.FirstOrDefault(r => r.Region.Equals(region, StringComparison.OrdinalIgnoreCase));
            return regionData?.TotalValue ?? 0;
        }

        public List<ContractTypeBreakdown> GetContractTypeDistribution()
        {
            return ContractTypes?
                .OrderByDescending(t => t.TotalValue)
                .ToList() ?? new List<ContractTypeBreakdown>();
        }

        public List<IndustryBreakdown> GetIndustryDistribution()
        {
            return IndustryData?
                .OrderByDescending(i => i.TotalValue)
                .ToList() ?? new List<IndustryBreakdown>();
        }
    }

    // Supporting Models
    public class MonthlyContractData
    {
        public DateTime Month { get; set; }
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public int ActiveCompanies { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayValue => $"{TotalValue:C0}";
    }

    public class YearlyContractData
    {
        public int Year { get; set; }
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageContractValue { get; set; }
        public string DisplayValue => $"{TotalValue:C0}";
        public string DisplayAverageContractValue => $"{AverageContractValue:C0}";
    }

    public class QuarterlyContractData
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public string DisplayQuarter => $"Q{Quarter} {Year}";
        public string DisplayValue => $"{TotalValue:C0}";
    }

    public class TopContractor
    {
        public string CompanyName { get; set; } = string.Empty;
        public string DUNSNumber { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalContractValue { get; set; }
        public decimal AverageContractValue { get; set; }
        public DateTime LastContractDate { get; set; }
        public string DisplayTotalValue => $"{TotalContractValue:C0}";
        public string DisplayAverageContractValue => $"{AverageContractValue:C0}";
        public string DisplayLastContractDate => LastContractDate.ToString("MMM dd, yyyy");
    }

    public class TopContractingAgency
    {
        public string AgencyName { get; set; } = string.Empty;
        public string AgencyCode { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalContractValue { get; set; }
        public decimal AverageContractValue { get; set; }
        public DateTime LastContractDate { get; set; }
        public string DisplayTotalValue => $"{TotalContractValue:C0}";
        public string DisplayAverageContractValue => $"{AverageContractValue:C0}";
        public string DisplayLastContractDate => LastContractDate.ToString("MMM dd, yyyy");
    }

    public class TopContractCategory
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageValue { get; set; }
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayAverageValue => $"{AverageValue:C0}";
    }

    public class RecentContract
    {
        public string ContractNumber { get; set; } = string.Empty;
        public string ContractorName { get; set; } = string.Empty;
        public string AgencyName { get; set; } = string.Empty;
        public string ContractTitle { get; set; } = string.Empty;
        public DateTime AwardDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public decimal ContractValue { get; set; }
        public ContractStatus Status { get; set; }
        public bool IsOnTime { get; set; }
        public decimal CostSavings { get; set; }
        public decimal QualityScore { get; set; }
        public string DisplayAwardDate => AwardDate.ToString("MMM dd, yyyy");
        public string DisplayCompletionDate => CompletionDate.ToString("MMM dd, yyyy");
        public string DisplayContractValue => $"{ContractValue:C0}";
        public string DisplayCostSavings => $"{CostSavings:C0}";
        public string DisplayQualityScore => $"{QualityScore:F1}";
    }

    public class NotableContract
    {
        public string ContractNumber { get; set; } = string.Empty;
        public string ContractorName { get; set; } = string.Empty;
        public string AgencyName { get; set; } = string.Empty;
        public string ContractTitle { get; set; } = string.Empty;
        public DateTime AwardDate { get; set; }
        public decimal ContractValue { get; set; }
        public string NotableReason { get; set; } = string.Empty;
        public string DisplayAwardDate => AwardDate.ToString("MMM dd, yyyy");
        public string DisplayContractValue => $"{ContractValue:C0}";
    }

    public class StateContractData
    {
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageValue { get; set; }
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayAverageValue => $"{AverageValue:C0}";
    }

    public class RegionContractData
    {
        public string Region { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageValue { get; set; }
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayAverageValue => $"{AverageValue:C0}";
    }

    public class ContractTypeBreakdown
    {
        public string ContractType { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayPercentage => $"{PercentageOfTotal:F1}%";
    }

    public class IndustryBreakdown
    {
        public string Industry { get; set; } = string.Empty;
        public string NAICSCode { get; set; } = string.Empty;
        public int TotalContracts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public string DisplayTotalValue => $"{TotalValue:C0}";
        public string DisplayPercentage => $"{PercentageOfTotal:F1}%";
    }

    public enum ContractStatus
    {
        Awarded,
        InProgress,
        Completed,
        Terminated,
        Suspended
    }
}
