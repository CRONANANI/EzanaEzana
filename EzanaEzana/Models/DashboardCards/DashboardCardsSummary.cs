using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.Models.DashboardCards
{
    /// <summary>
    /// Comprehensive model containing all dashboard cards data
    /// </summary>
    public class DashboardCardsSummary
    {
        /// <summary>
        /// Portfolio Value card data
        /// </summary>
        public PortfolioValueCard PortfolioValue { get; set; } = new();

        /// <summary>
        /// Today's P&L card data
        /// </summary>
        public TodaysPnlCard TodaysPnl { get; set; } = new();

        /// <summary>
        /// Top Performer card data
        /// </summary>
        public TopPerformerCard TopPerformer { get; set; } = new();

        /// <summary>
        /// Risk Score card data
        /// </summary>
        public RiskScoreCard RiskScore { get; set; } = new();

        /// <summary>
        /// Monthly Dividends card data
        /// </summary>
        public MonthlyDividendsCard MonthlyDividends { get; set; } = new();

        /// <summary>
        /// Asset Allocation card data
        /// </summary>
        public AssetAllocationCard AssetAllocation { get; set; } = new();

        /// <summary>
        /// When all data was last refreshed
        /// </summary>
        public DateTime LastRefreshed { get; set; } = DateTime.Now;

        /// <summary>
        /// Overall portfolio health score (0-100)
        /// </summary>
        [Range(0, 100)]
        public decimal PortfolioHealthScore { get; set; }

        /// <summary>
        /// Portfolio health status
        /// </summary>
        public PortfolioHealthStatus HealthStatus { get; set; }

        /// <summary>
        /// Key insights and recommendations
        /// </summary>
        public List<PortfolioInsight> Insights { get; set; } = new();

        /// <summary>
        /// Alerts and notifications
        /// </summary>
        public List<PortfolioAlert> Alerts { get; set; } = new();

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public DashboardSummaryDisplay DisplayValues => new()
        {
            LastRefreshedFormatted = LastRefreshed.ToString("MMM dd, yyyy HH:mm"),
            PortfolioHealthScoreFormatted = $"{PortfolioHealthScore:F0}/100",
            HealthStatusFormatted = HealthStatus.ToString().Replace("_", " "),
            HealthColor = GetHealthColor(),
            TotalAlerts = Alerts.Count,
            CriticalAlerts = Alerts.Count(a => a.Severity == AlertSeverity.Critical),
            HighPriorityAlerts = Alerts.Count(a => a.Severity == AlertSeverity.High),
            TotalInsights = Insights.Count
        };

        private string GetHealthColor()
        {
            return PortfolioHealthScore switch
            {
                >= 80 => "#10B981", // Green
                >= 60 => "#34D399", // Light Green
                >= 40 => "#F59E0B", // Yellow
                >= 20 => "#F97316", // Orange
                _ => "#EF4444"      // Red
            };
        }

        /// <summary>
        /// Calculate overall portfolio health score
        /// </summary>
        public void CalculatePortfolioHealthScore()
        {
            var scores = new List<decimal>();

            // Risk score (lower is better, so invert)
            if (RiskScore.Score > 0)
            {
                scores.Add((10 - RiskScore.Score) * 10); // Convert to 0-100 scale
            }

            // Diversification score
            scores.Add(AssetAllocation.DiversificationScore);

            // P&L performance (positive P&L gets higher score)
            if (TodaysPnl.TodayPnl >= 0)
            {
                scores.Add(80 + (TodaysPnl.TodayPnlPercentage * 2)); // Cap at 100
            }
            else
            {
                scores.Add(Math.Max(0, 80 + (TodaysPnl.TodayPnlPercentage * 2)));
            }

            // Dividend consistency (positive change gets higher score)
            if (MonthlyDividends.ChangeAmount >= 0)
            {
                scores.Add(80 + (MonthlyDividends.ChangePercentage * 2)); // Cap at 100
            }
            else
            {
                scores.Add(Math.Max(0, 80 + (MonthlyDividends.ChangePercentage * 2)));
            }

            PortfolioHealthScore = scores.Any() ? scores.Average() : 0;
            HealthStatus = GetHealthStatus();
        }

        private PortfolioHealthStatus GetHealthStatus()
        {
            return PortfolioHealthScore switch
            {
                >= 80 => PortfolioHealthStatus.Excellent,
                >= 60 => PortfolioHealthStatus.Good,
                >= 40 => PortfolioHealthStatus.Fair,
                >= 20 => PortfolioHealthStatus.Poor,
                _ => PortfolioHealthStatus.Critical
            };
        }
    }

    /// <summary>
    /// Display-specific values for the dashboard summary
    /// </summary>
    public class DashboardSummaryDisplay
    {
        public string LastRefreshedFormatted { get; set; } = string.Empty;
        public string PortfolioHealthScoreFormatted { get; set; } = string.Empty;
        public string HealthStatusFormatted { get; set; } = string.Empty;
        public string HealthColor { get; set; } = string.Empty;
        public int TotalAlerts { get; set; }
        public int CriticalAlerts { get; set; }
        public int HighPriorityAlerts { get; set; }
        public int TotalInsights { get; set; }
    }

    /// <summary>
    /// Portfolio health status
    /// </summary>
    public enum PortfolioHealthStatus
    {
        [Display(Name = "Critical")]
        Critical,
        [Display(Name = "Poor")]
        Poor,
        [Display(Name = "Fair")]
        Fair,
        [Display(Name = "Good")]
        Good,
        [Display(Name = "Excellent")]
        Excellent
    }

    /// <summary>
    /// Portfolio insight or recommendation
    /// </summary>
    public class PortfolioInsight
    {
        /// <summary>
        /// Insight title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Insight description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Insight category
        /// </summary>
        public InsightCategory Category { get; set; }

        /// <summary>
        /// Priority level
        /// </summary>
        public InsightPriority Priority { get; set; }

        /// <summary>
        /// When the insight was generated
        /// </summary>
        public DateTime GeneratedAt { get; set; }

        /// <summary>
        /// Related ticker symbols
        /// </summary>
        public List<string> RelatedTickers { get; set; } = new();

        /// <summary>
        /// Action items
        /// </summary>
        public List<string> ActionItems { get; set; } = new();
    }

    /// <summary>
    /// Portfolio alert or notification
    /// </summary>
    public class PortfolioAlert
    {
        /// <summary>
        /// Alert title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Alert message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Alert severity
        /// </summary>
        public AlertSeverity Severity { get; set; }

        /// <summary>
        /// Alert category
        /// </summary>
        public AlertCategory Category { get; set; }

        /// <summary>
        /// When the alert was generated
        /// </summary>
        public DateTime GeneratedAt { get; set; }

        /// <summary>
        /// Whether the alert has been acknowledged
        /// </summary>
        public bool IsAcknowledged { get; set; }

        /// <summary>
        /// Related ticker symbols
        /// </summary>
        public List<string> RelatedTickers { get; set; } = new();
    }

    /// <summary>
    /// Insight categories
    /// </summary>
    public enum InsightCategory
    {
        [Display(Name = "Performance")]
        Performance,
        [Display(Name = "Risk Management")]
        RiskManagement,
        [Display(Name = "Diversification")]
        Diversification,
        [Display(Name = "Income")]
        Income,
        [Display(Name = "Rebalancing")]
        Rebalancing,
        [Display(Name = "Market Opportunity")]
        MarketOpportunity
    }

    /// <summary>
    /// Insight priority levels
    /// </summary>
    public enum InsightPriority
    {
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "High")]
        High,
        [Display(Name = "Critical")]
        Critical
    }

    /// <summary>
    /// Alert severity levels
    /// </summary>
    public enum AlertSeverity
    {
        [Display(Name = "Info")]
        Info,
        [Display(Name = "Warning")]
        Warning,
        [Display(Name = "High")]
        High,
        [Display(Name = "Critical")]
        Critical
    }

    /// <summary>
    /// Alert categories
    /// </summary>
    public enum AlertCategory
    {
        [Display(Name = "Performance")]
        Performance,
        [Display(Name = "Risk")]
        Risk,
        [Display(Name = "Dividend")]
        Dividend,
        [Display(Name = "Rebalancing")]
        Rebalancing,
        [Display(Name = "Market")]
        Market,
        [Display(Name = "System")]
        System
    }
}
