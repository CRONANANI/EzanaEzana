using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Portfolio Management ViewModels
    public class CreatePortfolioViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }

    public class UpdatePortfolioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }

    // Portfolio Holdings ViewModels
    public class AddHoldingViewModel
    {
        [Required]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shares must be greater than 0")]
        public decimal Shares { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than 0")]
        public decimal AveragePrice { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateHoldingViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shares must be greater than 0")]
        public decimal Shares { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than 0")]
        public decimal AveragePrice { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    // Portfolio Transactions ViewModels
    public class AddTransactionViewModel
    {
        [Required]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        public string TransactionType { get; set; } = string.Empty; // Buy, Sell, Dividend, Split

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shares must be greater than 0")]
        public decimal Shares { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateTransactionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string TransactionType { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Shares must be greater than 0")]
        public decimal Shares { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public DateTime TransactionDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    // Watchlist ViewModels
    public class CreateWatchlistViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; }
    }

    public class UpdateWatchlistViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; }
    }

    public class AddWatchlistItemViewModel
    {
        [Required]
        public string Symbol { get; set; } = string.Empty;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public decimal TargetPrice { get; set; }
    }

    public class UpdateWatchlistItemViewModel
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public decimal TargetPrice { get; set; }
    }

    // Portfolio Analytics ViewModels
    public class PortfolioSummaryViewModel
    {
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalGainLoss { get; set; }
        public decimal TotalGainLossPercent { get; set; }
        public int TotalHoldings { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class PortfolioPerformanceViewModel
    {
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; } = string.Empty;
        public decimal TotalReturn { get; set; }
        public decimal AnnualizedReturn { get; set; }
        public decimal Volatility { get; set; }
        public decimal SharpeRatio { get; set; }
        public decimal MaxDrawdown { get; set; }
        public decimal Beta { get; set; }
        public List<MonthlyReturnViewModel> MonthlyReturns { get; set; } = new List<MonthlyReturnViewModel>();
    }

    public class MonthlyReturnViewModel
    {
        public DateTime Month { get; set; }
        public decimal Return { get; set; }
        public decimal CumulativeReturn { get; set; }
    }

    public class PortfolioAllocationViewModel
    {
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; } = string.Empty;
        public List<SectorAllocationViewModel> SectorAllocation { get; set; } = new List<SectorAllocationViewModel>();
        public List<IndustryAllocationViewModel> IndustryAllocation { get; set; } = new List<IndustryAllocationViewModel>();
        public List<MarketCapAllocationViewModel> MarketCapAllocation { get; set; } = new List<MarketCapAllocationViewModel>();
        public List<GeographicAllocationViewModel> GeographicAllocation { get; set; } = new List<GeographicAllocationViewModel>();
    }

    public class SectorAllocationViewModel
    {
        public string Sector { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int Holdings { get; set; }
    }

    public class IndustryAllocationViewModel
    {
        public string Industry { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int Holdings { get; set; }
    }

    public class MarketCapAllocationViewModel
    {
        public string MarketCapCategory { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int Holdings { get; set; }
    }

    public class GeographicAllocationViewModel
    {
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int Holdings { get; set; }
    }
}
