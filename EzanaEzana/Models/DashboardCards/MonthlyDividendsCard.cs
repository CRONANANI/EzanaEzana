using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Monthly Dividends dashboard card
    /// </summary>
    public class MonthlyDividendsCard
    {
        /// <summary>
        /// Monthly dividend amount
        /// </summary>
        [Display(Name = "Monthly Amount")]
        public decimal MonthlyAmount { get; set; }

        /// <summary>
        /// Previous month's dividend amount
        /// </summary>
        [Display(Name = "Previous Month")]
        public decimal PreviousMonthAmount { get; set; }

        /// <summary>
        /// Change in dividend amount from previous month
        /// </summary>
        [Display(Name = "Change Amount")]
        public decimal ChangeAmount { get; set; }

        /// <summary>
        /// Percentage change in dividends from previous month
        /// </summary>
        [Display(Name = "Change %")]
        public decimal ChangePercentage { get; set; }

        /// <summary>
        /// Next dividend payment date
        /// </summary>
        [Display(Name = "Next Payment")]
        public DateTime NextPaymentDate { get; set; }

        /// <summary>
        /// Upcoming dividend payments
        /// </summary>
        public List<DividendPayment> UpcomingPayments { get; set; } = new();

        /// <summary>
        /// Recent dividend payments
        /// </summary>
        public List<DividendPayment> RecentPayments { get; set; } = new();

        /// <summary>
        /// Year-to-date dividend total
        /// </summary>
        public decimal YearToDateTotal { get; set; }

        /// <summary>
        /// Projected annual dividend income
        /// </summary>
        public decimal ProjectedAnnualIncome { get; set; }

        /// <summary>
        /// Dividend yield percentage
        /// </summary>
        public decimal DividendYield { get; set; }

        /// <summary>
        /// Number of dividend-paying positions
        /// </summary>
        public int DividendPayingPositions { get; set; }

        /// <summary>
        /// Average dividend per position
        /// </summary>
        public decimal AverageDividendPerPosition { get; set; }

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public MonthlyDividendsDisplay DisplayValues => new()
        {
            MonthlyAmountFormatted = MonthlyAmount.ToString("C"),
            PreviousMonthAmountFormatted = PreviousMonthAmount.ToString("C"),
            ChangeAmountFormatted = ChangeAmount.ToString("C"),
            ChangePercentageFormatted = ChangePercentage.ToString("P2"),
            NextPaymentFormatted = NextPaymentDate.ToString("MMM dd, yyyy"),
            DaysUntilNextPayment = (NextPaymentDate - DateTime.Now).Days,
            IsPositiveChange = ChangeAmount >= 0,
            YearToDateFormatted = YearToDateTotal.ToString("C"),
            ProjectedAnnualFormatted = ProjectedAnnualIncome.ToString("C"),
            DividendYieldFormatted = DividendYield.ToString("P2"),
            AveragePerPositionFormatted = AverageDividendPerPosition.ToString("C")
        };
    }

    /// <summary>
    /// Display-specific values for the Monthly Dividends card
    /// </summary>
    public class MonthlyDividendsDisplay
    {
        public string MonthlyAmountFormatted { get; set; } = string.Empty;
        public string PreviousMonthAmountFormatted { get; set; } = string.Empty;
        public string ChangeAmountFormatted { get; set; } = string.Empty;
        public string ChangePercentageFormatted { get; set; } = string.Empty;
        public string NextPaymentFormatted { get; set; } = string.Empty;
        public int DaysUntilNextPayment { get; set; }
        public bool IsPositiveChange { get; set; }
        public string YearToDateFormatted { get; set; } = string.Empty;
        public string ProjectedAnnualFormatted { get; set; } = string.Empty;
        public string DividendYieldFormatted { get; set; } = string.Empty;
        public string AveragePerPositionFormatted { get; set; } = string.Empty;
    }

    /// <summary>
    /// Individual dividend payment information
    /// </summary>
    public class DividendPayment
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
        /// Dividend amount
        /// </summary>
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment date
        /// </summary>
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Payment status
        /// </summary>
        [Display(Name = "Status")]
        public DividendPaymentStatus Status { get; set; }

        /// <summary>
        /// Dividend per share
        /// </summary>
        public decimal DividendPerShare { get; set; }

        /// <summary>
        /// Number of shares
        /// </summary>
        public int Shares { get; set; }

        /// <summary>
        /// Ex-dividend date
        /// </summary>
        public DateTime ExDividendDate { get; set; }

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string AmountFormatted => Amount.ToString("C");
        public string PaymentDateFormatted => PaymentDate.ToString("MMM dd, yyyy");
        public string DividendPerShareFormatted => DividendPerShare.ToString("C");
        public string ExDividendDateFormatted => ExDividendDate.ToString("MMM dd, yyyy");
    }

    /// <summary>
    /// Dividend payment status
    /// </summary>
    public enum DividendPaymentStatus
    {
        [Display(Name = "Upcoming")]
        Upcoming,
        [Display(Name = "Declared")]
        Declared,
        [Display(Name = "Paid")]
        Paid,
        [Display(Name = "Cancelled")]
        Cancelled,
        [Display(Name = "Suspended")]
        Suspended
    }
}
