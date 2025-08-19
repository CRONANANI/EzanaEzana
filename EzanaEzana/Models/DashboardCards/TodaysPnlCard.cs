using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Today's P&L dashboard card
    /// </summary>
    public class TodaysPnlCard
    {
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
        /// Total portfolio value for context
        /// </summary>
        [Display(Name = "Total Portfolio Value")]
        public decimal TotalPortfolioValue { get; set; }

        /// <summary>
        /// When the data was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Market session status
        /// </summary>
        public MarketSessionStatus MarketStatus { get; set; } = MarketSessionStatus.Closed;

        /// <summary>
        /// Previous day's P&L for comparison
        /// </summary>
        public decimal PreviousDayPnl { get; set; }

        /// <summary>
        /// Week-to-date P&L
        /// </summary>
        public decimal WeekToDatePnl { get; set; }

        /// <summary>
        /// Month-to-date P&L
        /// </summary>
        public decimal MonthToDatePnl { get; set; }

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public TodaysPnlDisplay DisplayValues => new()
        {
            TodayPnlFormatted = TodayPnl.ToString("C"),
            TodayPnlPercentageFormatted = TodayPnlPercentage.ToString("P2"),
            TotalPortfolioValueFormatted = TotalPortfolioValue.ToString("C"),
            LastUpdatedFormatted = LastUpdated.ToString("MMM dd, yyyy HH:mm"),
            IsPositivePnl = TodayPnl >= 0,
            PnlChangeFromPrevious = TodayPnl - PreviousDayPnl,
            PnlChangeFromPreviousFormatted = (TodayPnl - PreviousDayPnl).ToString("C"),
            IsPositiveChange = (TodayPnl - PreviousDayPnl) >= 0
        };
    }

    /// <summary>
    /// Display-specific values for the Today's P&L card
    /// </summary>
    public class TodaysPnlDisplay
    {
        public string TodayPnlFormatted { get; set; } = string.Empty;
        public string TodayPnlPercentageFormatted { get; set; } = string.Empty;
        public string TotalPortfolioValueFormatted { get; set; } = string.Empty;
        public string LastUpdatedFormatted { get; set; } = string.Empty;
        public bool IsPositivePnl { get; set; }
        public decimal PnlChangeFromPrevious { get; set; }
        public string PnlChangeFromPreviousFormatted { get; set; } = string.Empty;
        public bool IsPositiveChange { get; set; }
    }

    /// <summary>
    /// Market session status
    /// </summary>
    public enum MarketSessionStatus
    {
        [Display(Name = "Pre-Market")]
        PreMarket,
        [Display(Name = "Market Open")]
        Open,
        [Display(Name = "Market Closed")]
        Closed,
        [Display(Name = "After Hours")]
        AfterHours,
        [Display(Name = "Holiday")]
        Holiday
    }
}
