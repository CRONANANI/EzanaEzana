using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Historical portfolio data point for performance tracking
    /// </summary>
    public class PortfolioDataPoint
    {
        /// <summary>
        /// Date of the data point
        /// </summary>
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Portfolio value on this date
        /// </summary>
        [Display(Name = "Portfolio Value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Return amount from baseline
        /// </summary>
        [Display(Name = "Return")]
        public decimal Return { get; set; }

        /// <summary>
        /// Return percentage from baseline
        /// </summary>
        [Display(Name = "Return %")]
        public decimal ReturnPercentage { get; set; }

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string ValueFormatted => Value.ToString("C");
        public string ReturnFormatted => Return.ToString("C");
        public string ReturnPercentageFormatted => ReturnPercentage.ToString("P2");
        public string DateFormatted => Date.ToString("MMM dd, yyyy");
        public bool IsPositiveReturn => Return >= 0;
        public bool IsPositiveReturnPercentage => ReturnPercentage >= 0;
    }
}
