using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EzanaEzana.Models.PublicOfficials
{
    public class LobbyingActivityCard
    {
        // Core Data Properties
        public int TotalLobbyingReports { get; set; }
        public decimal TotalSpending { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ActiveLobbyingFirms { get; set; }
        public int UniqueCompanies { get; set; }

        // Spending Metrics
        public decimal AverageSpending { get; set; }
        public decimal HighestSpending { get; set; }
        public decimal LowestSpending { get; set; }
        public decimal TotalDisbursements { get; set; }

        // Time-based Analysis
        public List<MonthlyLobbyingData> MonthlyData { get; set; } = new List<MonthlyLobbyingData>();
        public List<YearlyLobbyingData> YearlyData { get; set; } = new List<YearlyLobbyingData>();
        public List<QuarterlyLobbyingData> QuarterlyData { get; set; } = new List<QuarterlyLobbyingData>();

        // Top Performers
        public List<TopLobbyingFirm> TopFirms { get; set; } = new List<TopLobbyingFirm>();
        public List<TopLobbyingCompany> TopCompanies { get; set; } = new List<TopLobbyingCompany>();
        public List<TopLobbyingIssue> TopIssues { get; set; } = new List<TopLobbyingIssue>();

        // Recent Activity
        public List<RecentLobbyingReport> RecentReports { get; set; } = new List<RecentLobbyingReport>();
        public List<NotableLobbyingActivity> NotableActivities { get; set; } = new List<NotableLobbyingActivity>();

        // Issue and Industry Analysis
        public List<IssueBreakdown> IssueData { get; set; } = new List<IssueBreakdown>();
        public List<IndustryLobbyingData> IndustryData { get; set; } = new List<IndustryLobbyingData>();
        public List<LobbyingCategory> CategoryData { get; set; } = new List<LobbyingCategory>();

        // Geographic Distribution
        public List<StateLobbyingData> StateData { get; set; } = new List<StateLobbyingData>();
        public List<RegionLobbyingData> RegionData { get; set; } = new List<RegionLobbyingData>();

        // Performance and Compliance
        public decimal ComplianceRate { get; set; }
        public int LateReports { get; set; }
        public int IncompleteReports { get; set; }
        public List<ComplianceAlert> ComplianceAlerts { get; set; } = new List<ComplianceAlert>();

        // Display Properties
        public string DisplayTotalReports => $"{TotalLobbyingReports:N0}";
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayComplianceRate => $"{ComplianceRate:F1}%";
        public string DisplayLateReports => $"{LateReports:N0}";

        // Calculation Methods
        public void CalculateMetrics()
        {
            if (RecentReports?.Any() == true)
            {
                AverageSpending = RecentReports.Average(r => r.TotalSpending);
                HighestSpending = RecentReports.Max(r => r.TotalSpending);
                LowestSpending = RecentReports.Min(r => r.TotalSpending);
                TotalDisbursements = RecentReports.Sum(r => r.TotalSpending);
            }

            if (MonthlyData?.Any() == true)
            {
                var currentMonth = MonthlyData.OrderByDescending(m => m.Month).FirstOrDefault();
                if (currentMonth != null)
                {
                    TotalLobbyingReports = currentMonth.TotalReports;
                    TotalSpending = currentMonth.TotalSpending;
                }
            }

            CalculateComplianceMetrics();
            CalculateIssueMetrics();
        }

        private void CalculateComplianceMetrics()
        {
            if (RecentReports?.Any() == true)
            {
                var compliantReports = RecentReports.Where(r => r.IsCompliant);
                var lateReports = RecentReports.Where(r => r.IsLate);
                var incompleteReports = RecentReports.Where(r => !r.IsComplete);

                if (RecentReports.Any())
                {
                    ComplianceRate = (decimal)compliantReports.Count() / RecentReports.Count * 100;
                    LateReports = lateReports.Count();
                    IncompleteReports = incompleteReports.Count();
                }
            }
        }

        private void CalculateIssueMetrics()
        {
            if (IssueData?.Any() == true)
            {
                var totalSpending = IssueData.Sum(i => i.TotalSpending);
                foreach (var issue in IssueData)
                {
                    if (totalSpending > 0)
                    {
                        issue.PercentageOfTotal = (issue.TotalSpending / totalSpending) * 100;
                    }
                }
            }
        }

        public List<MonthlyLobbyingData> GetTopMonths(int count = 5)
        {
            return MonthlyData?
                .OrderByDescending(m => m.TotalSpending)
                .Take(count)
                .ToList() ?? new List<MonthlyLobbyingData>();
        }

        public List<TopLobbyingFirm> GetTopFirmsBySpending(int count = 10)
        {
            return TopFirms?
                .OrderByDescending(f => f.TotalSpending)
                .Take(count)
                .ToList() ?? new List<TopLobbyingFirm>();
        }

        public List<TopLobbyingCompany> GetTopCompaniesBySpending(int count = 10)
        {
            return TopCompanies?
                .OrderByDescending(c => c.TotalSpending)
                .Take(count)
                .ToList() ?? new List<TopLobbyingCompany>();
        }

        public List<TopLobbyingIssue> GetTopIssuesBySpending(int count = 10)
        {
            return TopIssues?
                .OrderByDescending(i => i.TotalSpending)
                .Take(count)
                .ToList() ?? new List<TopLobbyingIssue>();
        }

        public decimal GetMonthlyGrowthRate()
        {
            if (MonthlyData?.Count < 2) return 0;

            var currentMonth = MonthlyData.OrderByDescending(m => m.Month).First();
            var previousMonth = MonthlyData.OrderByDescending(m => m.Month).Skip(1).First();

            if (previousMonth.TotalSpending == 0) return 0;

            return ((currentMonth.TotalSpending - previousMonth.TotalSpending) / previousMonth.TotalSpending) * 100;
        }

        public string GetLobbyingTrend()
        {
            var growthRate = GetMonthlyGrowthRate();
            
            if (growthRate > 20) return "Strongly Increasing";
            if (growthRate > 10) return "Increasing";
            if (growthRate > -10) return "Stable";
            if (growthRate > -20) return "Decreasing";
            return "Strongly Decreasing";
        }

        public List<IssueBreakdown> GetTopIssues(int count = 10)
        {
            return IssueData?
                .OrderByDescending(i => i.TotalSpending)
                .Take(count)
                .ToList() ?? new List<IssueBreakdown>();
        }

        public List<IndustryLobbyingData> GetTopIndustries(int count = 10)
        {
            return IndustryData?
                .OrderByDescending(i => i.TotalSpending)
                .Take(count)
                .ToList() ?? new List<IndustryLobbyingData>();
        }

        public List<StateLobbyingData> GetTopStates(int count = 10)
        {
            return StateData?
                .OrderByDescending(s => s.TotalSpending)
                .Take(count)
                .ToList() ?? new List<StateLobbyingData>();
        }

        public List<ComplianceAlert> GetHighPriorityAlerts()
        {
            return ComplianceAlerts?
                .Where(c => c.Priority == CompliancePriority.High)
                .OrderByDescending(c => c.Severity)
                .ToList() ?? new List<ComplianceAlert>();
        }

        public decimal GetIndustrySpending(string industry)
        {
            var industryData = IndustryData?.FirstOrDefault(i => i.Industry.Equals(industry, StringComparison.OrdinalIgnoreCase));
            return industryData?.TotalSpending ?? 0;
        }

        public List<RecentLobbyingReport> GetHighSpendingReports(decimal threshold = 100000)
        {
            return RecentReports?
                .Where(r => r.TotalSpending >= threshold)
                .OrderByDescending(r => r.TotalSpending)
                .ToList() ?? new List<RecentLobbyingReport>();
        }
    }

    // Supporting Models
    public class MonthlyLobbyingData
    {
        public DateTime Month { get; set; }
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public int ActiveFirms { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplaySpending => $"{TotalSpending:C0}";
    }

    public class YearlyLobbyingData
    {
        public int Year { get; set; }
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplaySpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class QuarterlyLobbyingData
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public string DisplayQuarter => $"Q{Quarter} {Year}";
        public string DisplaySpending => $"{TotalSpending:C0}";
    }

    public class TopLobbyingFirm
    {
        public string FirmName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public DateTime LastReportDate { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
        public string DisplayLastReportDate => LastReportDate.ToString("MMM dd, yyyy");
    }

    public class TopLobbyingCompany
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public DateTime LastReportDate { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
        public string DisplayLastReportDate => LastReportDate.ToString("MMM dd, yyyy");
    }

    public class TopLobbyingIssue
    {
        public string IssueName { get; set; } = string.Empty;
        public string IssueCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class RecentLobbyingReport
    {
        public string ReportNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string LobbyingFirm { get; set; } = string.Empty;
        public string Issue { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalSpending { get; set; }
        public bool IsCompliant { get; set; }
        public bool IsLate { get; set; }
        public bool IsComplete { get; set; }
        public string DisplayReportDate => ReportDate.ToString("MMM dd, yyyy");
        public string DisplayDueDate => DueDate.ToString("MMM dd, yyyy");
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
    }

    public class NotableLobbyingActivity
    {
        public string CompanyName { get; set; } = string.Empty;
        public string LobbyingFirm { get; set; } = string.Empty;
        public string Issue { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public decimal TotalSpending { get; set; }
        public string NotableReason { get; set; } = string.Empty;
        public string DisplayReportDate => ReportDate.ToString("MMM dd, yyyy");
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
    }

    public class IssueBreakdown
    {
        public string IssueName { get; set; } = string.Empty;
        public string IssueCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayPercentage => $"{PercentageOfTotal:F1}%";
    }

    public class IndustryLobbyingData
    {
        public string Industry { get; set; } = string.Empty;
        public string NAICSCode { get; set; } = string.Empty;
        public int TotalCompanies { get; set; }
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class LobbyingCategory
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class StateLobbyingData
    {
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class RegionLobbyingData
    {
        public string Region { get; set; } = string.Empty;
        public int TotalReports { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal AverageSpending { get; set; }
        public string DisplayTotalSpending => $"{TotalSpending:C0}";
        public string DisplayAverageSpending => $"{AverageSpending:C0}";
    }

    public class ComplianceAlert
    {
        public string CompanyName { get; set; } = string.Empty;
        public string LobbyingFirm { get; set; } = string.Empty;
        public string Issue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CompliancePriority Priority { get; set; }
        public decimal Severity { get; set; }
        public DateTime AlertDate { get; set; }
        public string DisplayAlertDate => AlertDate.ToString("MMM dd, yyyy");
        public string DisplaySeverity => $"{Severity:F1}";
    }

    public enum CompliancePriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
