using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Individual portfolio holding information
    /// </summary>
    public class PortfolioHolding
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
        /// Current value of the holding
        /// </summary>
        [Display(Name = "Current Value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Number of shares owned
        /// </summary>
        [Display(Name = "Shares")]
        public int Shares { get; set; }

        /// <summary>
        /// Return percentage
        /// </summary>
        [Display(Name = "Return %")]
        public decimal Return { get; set; }

        /// <summary>
        /// Sector/industry classification
        /// </summary>
        [Display(Name = "Sector")]
        public string Sector { get; set; } = string.Empty;

        /// <summary>
        /// When the data was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string ValueFormatted => Value.ToString("C");
        public string ReturnFormatted => Return.ToString("P2");
        public string SharesFormatted => Shares.ToString("N0");
        public string LastUpdatedFormatted => LastUpdated.ToString("MMM dd, yyyy");
        public bool IsPositiveReturn => Return >= 0;
    }
}
