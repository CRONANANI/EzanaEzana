using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Total Portfolio Value dashboard card
    /// </summary>
    public class PortfolioValueCard
    {
        /// <summary>
        /// Total current portfolio value
        /// </summary>
        [Display(Name = "Total Portfolio Value")]
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Total return amount (absolute value)
        /// </summary>
        [Display(Name = "Total Return")]
        public decimal TotalReturn { get; set; }

        /// <summary>
        /// Total return percentage
        /// </summary>
        [Display(Name = "Return Percentage")]
        public decimal ReturnPercentage { get; set; }

        /// <summary>
        /// Today's profit/loss amount
        /// </summary>
        [Display(Name = "Today's P&L")]
        public decimal TodayPnl { get; set; }

        /// <summary>
        /// Today's profit/loss percentage
        /// </summary>
        [Display(Name = "Today's P&L %")]
        public decimal TodayPnlPercentage { get; set; }

        /// <summary>
        /// When the data was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Top holdings in the portfolio
        /// </summary>
        public List<PortfolioHolding> TopHoldings { get; set; } = new();

        /// <summary>
        /// Recent portfolio trades
        /// </summary>
        public List<PortfolioTrade> RecentTrades { get; set; } = new();

        /// <summary>
        /// Portfolio performance over time
        /// </summary>
        public List<PortfolioDataPoint> PerformanceHistory { get; set; } = new();

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public PortfolioValueDisplay DisplayValues => new()
        {
            TotalValueFormatted = TotalValue.ToString("C"),
            TotalReturnFormatted = TotalReturn.ToString("C"),
            ReturnPercentageFormatted = ReturnPercentage.ToString("P2"),
            TodayPnlFormatted = TodayPnl.ToString("C"),
            TodayPnlPercentageFormatted = TodayPnlPercentage.ToString("P2"),
            LastUpdatedFormatted = LastUpdated.ToString("MMM dd, yyyy HH:mm"),
            IsPositiveReturn = TotalReturn >= 0,
            IsPositiveTodayPnl = TodayPnl >= 0
        };
    }

    /// <summary>
    /// Display-specific values for the Portfolio Value card
    /// </summary>
    public class PortfolioValueDisplay
    {
        public string TotalValueFormatted { get; set; } = string.Empty;
        public string TotalReturnFormatted { get; set; } = string.Empty;
        public string ReturnPercentageFormatted { get; set; } = string.Empty;
        public string TodayPnlFormatted { get; set; } = string.Empty;
        public string TodayPnlPercentageFormatted { get; set; } = string.Empty;
        public string LastUpdatedFormatted { get; set; } = string.Empty;
        public bool IsPositiveReturn { get; set; }
        public bool IsPositiveTodayPnl { get; set; }
    }
}
