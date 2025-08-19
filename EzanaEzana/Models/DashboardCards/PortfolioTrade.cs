using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Individual portfolio trade information
    /// </summary>
    public class PortfolioTrade
    {
        /// <summary>
        /// Trade date
        /// </summary>
        [Display(Name = "Date")]
        public string Date { get; set; } = string.Empty;

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
        /// Trade type (BUY/SELL)
        /// </summary>
        [Display(Name = "Type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Trade amount in dollars
        /// </summary>
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Number of shares traded
        /// </summary>
        [Display(Name = "Shares")]
        public int Shares { get; set; }

        /// <summary>
        /// Trade price per share
        /// </summary>
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string AmountFormatted => Amount.ToString("C");
        public string PriceFormatted => Price.ToString("C");
        public string SharesFormatted => Shares.ToString("N0");
        public string DateFormatted => DateTime.Parse(Date).ToString("MMM dd, yyyy");
        public bool IsBuy => Type.Equals("BUY", StringComparison.OrdinalIgnoreCase);
        public bool IsSell => Type.Equals("SELL", StringComparison.OrdinalIgnoreCase);
    }
}
