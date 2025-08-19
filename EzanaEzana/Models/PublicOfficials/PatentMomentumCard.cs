using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ezana.Models.PublicOfficials
{
    public class PatentMomentumCard
    {
        // Core Data Properties
        public int TotalPatents { get; set; }
        public int ActivePatents { get; set; }
        public int PendingPatents { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UniqueCompanies { get; set; }
        public int UniqueInventors { get; set; }

        // Patent Metrics
        public decimal AveragePatentsPerCompany { get; set; }
        public decimal AveragePatentsPerInventor { get; set; }
        public int HighestPatentCount { get; set; }
        public int LowestPatentCount { get; set; }

        // Time-based Analysis
        public List<MonthlyPatentData> MonthlyData { get; set; } = new List<MonthlyPatentData>();
        public List<YearlyPatentData> YearlyData { get; set; } = new List<YearlyPatentData>();
        public List<QuarterlyPatentData> QuarterlyData { get; set; } = new List<QuarterlyPatentData>();

        // Top Performers
        public List<TopPatentCompany> TopCompanies { get; set; } = new List<TopPatentCompany>();
        public List<TopInventor> TopInventors { get; set; } = new List<TopInventor>();
        public List<TopPatentTechnology> TopTechnologies { get; set; } = new List<TopPatentTechnology>();

        // Recent Activity
        public List<RecentPatent> RecentPatents { get; set; } = new List<RecentPatent>();
        public List<NotablePatent> NotablePatents { get; set; } = new List<NotablePatent>();

        // Technology and Industry Analysis
        public List<TechnologyBreakdown> TechnologyData { get; set; } = new List<TechnologyBreakdown>();
        public List<IndustryPatentData> IndustryData { get; set; } = new List<IndustryPatentData>();
        public List<PatentCategory> CategoryData { get; set; } = new List<PatentCategory>();

        // Geographic Distribution
        public List<StatePatentData> StateData { get; set; } = new List<StatePatentData>();
        public List<CountryPatentData> CountryData { get; set; } = new List<CountryPatentData>();

        // Patent Quality and Impact
        public decimal AveragePatentQuality { get; set; }
        public decimal CitationRate { get; set; }
        public decimal LitigationRate { get; set; }
        public List<PatentQualityMetric> QualityMetrics { get; set; } = new List<PatentQualityMetric>();

        // Innovation Trends
        public List<InnovationTrend> InnovationTrends { get; set; } = new List<InnovationTrend>();
        public List<EmergingTechnology> EmergingTechnologies { get; set; } = new List<EmergingTechnology>();

        // Display Properties
        public string DisplayTotalPatents => $"{TotalPatents:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayPendingPatents => $"{PendingPatents:N0}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayAveragePatentsPerCompany => $"{AveragePatentsPerCompany:F1}";
        public string DisplayAveragePatentsPerInventor => $"{AveragePatentsPerInventor:F1}";
        public string DisplayAveragePatentQuality => $"{AveragePatentQuality:F1}";
        public string DisplayCitationRate => $"{CitationRate:F1}%";
        public string DisplayLitigationRate => $"{LitigationRate:F1}%";

        // Calculation Methods
        public void CalculateMetrics()
        {
            if (TopCompanies?.Any() == true)
            {
                AveragePatentsPerCompany = (decimal)TotalPatents / UniqueCompanies;
                HighestPatentCount = TopCompanies.Max(c => c.PatentCount);
                LowestPatentCount = TopCompanies.Min(c => c.PatentCount);
            }

            if (TopInventors?.Any() == true)
            {
                AveragePatentsPerInventor = (decimal)TotalPatents / UniqueInventors;
            }

            if (MonthlyData?.Any() == true)
            {
                var currentMonth = MonthlyData.OrderByDescending(m => m.Month).FirstOrDefault();
                if (currentMonth != null)
                {
                    TotalPatents = currentMonth.TotalPatents;
                    ActivePatents = currentMonth.ActivePatents;
                    PendingPatents = currentMonth.PendingPatents;
                }
            }

            CalculateQualityMetrics();
            CalculateInnovationMetrics();
        }

        private void CalculateQualityMetrics()
        {
            if (QualityMetrics?.Any() == true)
            {
                AveragePatentQuality = QualityMetrics.Average(q => q.QualityScore);
                CitationRate = QualityMetrics.Average(q => q.CitationRate);
                LitigationRate = QualityMetrics.Average(q => q.LitigationRate);
            }
        }

        private void CalculateInnovationMetrics()
        {
            if (TechnologyData?.Any() == true)
            {
                var totalPatents = TechnologyData.Sum(t => t.PatentCount);
                foreach (var tech in TechnologyData)
                {
                    if (totalPatents > 0)
                    {
                        tech.PercentageOfTotal = (decimal)tech.PatentCount / totalPatents * 100;
                    }
                }
            }
        }

        public List<MonthlyPatentData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalPatents)
                .Take(count)
                .ToList() ?? new List<MonthlyPatentData>();
        }

        public List<TopPatentCompany> GetTopCompaniesByPatents(int count = 10)
        {
            return TopCompanies?
                .OrderByDescending(c => c.PatentCount)
                .Take(count)
                .ToList() ?? new List<TopPatentCompany>();
        }

        public List<TopInventor> GetTopInventorsByPatents(int count = 10)
        {
            return TopInventors?
                .OrderByDescending(i => i.PatentCount)
                .Take(count)
                .ToList() ?? new List<TopInventor>();
        }

        public List<TopPatentTechnology> GetTopTechnologies(int count = 10)
        {
            return TopTechnologies?
                .OrderByDescending(t => t.PatentCount)
                .Take(count)
                .ToList() ?? new List<TopPatentTechnology>();
        }

        public decimal GetMonthlyGrowthRate()
        {
            if (MonthlyData?.Count < 2) return 0;

            var currentMonth = MonthlyData.OrderByDescending(m => m.Month).First();
            var previousMonth = MonthlyData.OrderByDescending(m => m.Month).Skip(1).First();

            if (previousMonth.TotalPatents == 0) return 0;

            return ((currentMonth.TotalPatents - previousMonth.TotalPatents) / (decimal)previousMonth.TotalPatents) * 100;
        }

        public string GetPatentTrend()
        {
            var growthRate = GetMonthlyGrowthRate();
            
            if (growthRate > 25) return "Explosive Growth";
            if (growthRate > 15) return "Strong Growth";
            if (growthRate > 5) return "Moderate Growth";
            if (growthRate > -5) return "Stable";
            if (growthRate > -15) return "Declining";
            return "Significantly Declining";
        }

        public List<TechnologyBreakdown> GetTopTechnologies(int count = 10)
        {
            return TechnologyData?
                .OrderByDescending(t => t.PatentCount)
                .Take(count)
                .ToList() ?? new List<TechnologyBreakdown>();
        }

        public List<IndustryPatentData> GetTopIndustries(int count = 10)
        {
            return IndustryData?
                .OrderByDescending(i => i.PatentCount)
                .Take(count)
                .ToList() ?? new List<IndustryPatentData>();
        }

        public List<StatePatentData> GetTopStates(int count = 10)
        {
            return StateData?
                .OrderByDescending(s => s.PatentCount)
                .Take(count)
                .ToList() ?? new List<StatePatentData>();
        }

        public List<PatentQualityMetric> GetHighQualityPatents(decimal threshold = 8.0m)
        {
            return QualityMetrics?
                .Where(q => q.QualityScore >= threshold)
                .OrderByDescending(q => q.QualityScore)
                .ToList() ?? new List<PatentQualityMetric>();
        }

        public List<EmergingTechnology> GetEmergingTechnologies()
        {
            return EmergingTechnologies?
                .OrderByDescending(e => e.GrowthRate)
                .ToList() ?? new List<EmergingTechnology>();
        }

        public decimal GetTechnologyPatentCount(string technology)
        {
            var techData = TechnologyData?.FirstOrDefault(t => t.TechnologyName.Equals(technology, StringComparison.OrdinalIgnoreCase));
            return techData?.PatentCount ?? 0;
        }

        public List<RecentPatent> GetHighImpactPatents(decimal qualityThreshold = 8.0m)
        {
            return RecentPatents?
                .Where(p => p.QualityScore >= qualityThreshold)
                .OrderByDescending(p => p.QualityScore)
                .ToList() ?? new List<RecentPatent>();
        }
    }

    // Supporting Models
    public class MonthlyPatentData
    {
        public DateTime Month { get; set; }
        public int TotalPatents { get; set; }
        public int ActivePatents { get; set; }
        public int PendingPatents { get; set; }
        public int NewPatents { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayTotalPatents => $"{TotalPatents:N0}";
    }

    public class YearlyPatentData
    {
        public int Year { get; set; }
        public int TotalPatents { get; set; }
        public int ActivePatents { get; set; }
        public int PendingPatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayTotalPatents => $"{TotalPatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class QuarterlyPatentData
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int TotalPatents { get; set; }
        public int ActivePatents { get; set; }
        public int PendingPatents { get; set; }
        public string DisplayQuarter => $"Q{Quarter} {Year}";
        public string DisplayTotalPatents => $"{TotalPatents:N0}";
    }

    public class TopPatentCompany
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public int PendingPatents { get; set; }
        public decimal AverageQuality { get; set; }
        public DateTime LastPatentDate { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
        public string DisplayLastPatentDate => LastPatentDate.ToString("MMM dd, yyyy");
    }

    public class TopInventor
    {
        public string InventorName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public DateTime LastPatentDate { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
        public string DisplayLastPatentDate => LastPatentDate.ToString("MMM dd, yyyy");
    }

    public class TopPatentTechnology
    {
        public string TechnologyName { get; set; } = string.Empty;
        public string TechnologyCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class RecentPatent
    {
        public string PatentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public List<string> Inventors { get; set; } = new List<string>();
        public DateTime FilingDate { get; set; }
        public DateTime IssueDate { get; set; }
        public string Technology { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal QualityScore { get; set; }
        public string DisplayFilingDate => FilingDate.ToString("MMM dd, yyyy");
        public string DisplayIssueDate => IssueDate.ToString("MMM dd, yyyy");
        public string DisplayQualityScore => $"{QualityScore:F1}";
        public string DisplayInventors => string.Join(", ", Inventors);
    }

    public class NotablePatent
    {
        public string PatentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Technology { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string NotableReason { get; set; } = string.Empty;
        public decimal QualityScore { get; set; }
        public string DisplayIssueDate => IssueDate.ToString("MMM dd, yyyy");
        public string DisplayQualityScore => $"{QualityScore:F1}";
    }

    public class TechnologyBreakdown
    {
        public string TechnologyName { get; set; } = string.Empty;
        public string TechnologyCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayPercentage => $"{PercentageOfTotal:F1}%";
    }

    public class IndustryPatentData
    {
        public string Industry { get; set; } = string.Empty;
        public string NAICSCode { get; set; } = string.Empty;
        public int TotalCompanies { get; set; }
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class PatentCategory
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class StatePatentData
    {
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class CountryPatentData
    {
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public int ActivePatents { get; set; }
        public decimal AverageQuality { get; set; }
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayActivePatents => $"{ActivePatents:N0}";
        public string DisplayAverageQuality => $"{AverageQuality:F1}";
    }

    public class PatentQualityMetric
    {
        public string PatentNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public decimal QualityScore { get; set; }
        public decimal CitationRate { get; set; }
        public decimal LitigationRate { get; set; }
        public int ForwardCitations { get; set; }
        public int BackwardCitations { get; set; }
        public string DisplayQualityScore => $"{QualityScore:F1}";
        public string DisplayCitationRate => $"{CitationRate:F1}%";
        public string DisplayLitigationRate => $"{LitigationRate:F1}%";
    }

    public class InnovationTrend
    {
        public string Technology { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public decimal GrowthRate { get; set; }
        public string Trend { get; set; } = string.Empty;
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayGrowthRate => $"{GrowthRate:F1}%";
    }

    public class EmergingTechnology
    {
        public string TechnologyName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PatentCount { get; set; }
        public decimal GrowthRate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DisplayPatentCount => $"{PatentCount:N0}";
        public string DisplayGrowthRate => $"{GrowthRate:F1}%";
    }
}
