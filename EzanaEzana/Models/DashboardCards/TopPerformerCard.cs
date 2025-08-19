using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Top Performer dashboard card
    /// </summary>
    public class TopPerformerCard
    {
        /// <summary>
        /// Stock ticker symbol
        /// </summary>
        [Display(Name = "Ticker")]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Company name
        /// </summary>
        [Display(Name = "Company")]
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Return amount in dollars
        /// </summary>
        [Display(Name = "Return Amount")]
        public decimal ReturnAmount { get; set; }

        /// <summary>
        /// Return percentage
        /// </summary>
        [Display(Name = "Return %")]
        public decimal ReturnPercentage { get; set; }

        /// <summary>
        /// Number of shares owned
        /// </summary>
        [Display(Name = "Shares")]
        public int Shares { get; set; }

        /// <summary>
        /// Current value of the holding
        /// </summary>
        [Display(Name = "Current Value")]
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// When the data was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Sector/industry classification
        /// </summary>
        public string Sector { get; set; } = string.Empty;

        /// <summary>
        /// Previous day's return for comparison
        /// </summary>
        public decimal PreviousDayReturn { get; set; }

        /// <summary>
        /// Week-to-date return
        /// </summary>
        public decimal WeekToDateReturn { get; set; }

        /// <summary>
        /// Month-to-date return
        /// </summary>
        public decimal MonthToDateReturn { get; set; }

        /// <summary>
        /// Stock price at close
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// Stock price change from previous close
        /// </summary>
        public decimal PriceChange { get; set; }

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public TopPerformerDisplay DisplayValues => new()
        {
            ReturnAmountFormatted = ReturnAmount.ToString("C"),
            ReturnPercentageFormatted = ReturnPercentage.ToString("P2"),
            CurrentValueFormatted = CurrentValue.ToString("C"),
            LastUpdatedFormatted = LastUpdated.ToString("MMM dd, yyyy HH:mm"),
            IsPositiveReturn = ReturnAmount >= 0,
            PriceChangeFormatted = PriceChange.ToString("C"),
            IsPositivePriceChange = PriceChange >= 0,
            SharesFormatted = Shares.ToString("N0")
        };
    }

    /// <summary>
    /// Display-specific values for the Top Performer card
    /// </summary>
    public class TopPerformerDisplay
    {
        public string ReturnAmountFormatted { get; set; } = string.Empty;
        public string ReturnPercentageFormatted { get; set; } = string.Empty;
        public string CurrentValueFormatted { get; set; } = string.Empty;
        public string LastUpdatedFormatted { get; set; } = string.Empty;
        public bool IsPositiveReturn { get; set; }
        public string PriceChangeFormatted { get; set; } = string.Empty;
        public bool IsPositivePriceChange { get; set; }
        public string SharesFormatted { get; set; } = string.Empty;
    }
}
