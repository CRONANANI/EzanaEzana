using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models
{
    // Market Data System
    public class MarketData
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal CurrentPrice { get; set; }
        
        public decimal PriceChange { get; set; }
        public decimal PriceChangePercent { get; set; }
        
        [Required]
        public long Volume { get; set; }
        
        public decimal? MarketCap { get; set; }
        public decimal? PE { get; set; }
        public decimal? PB { get; set; }
        public decimal? DividendYield { get; set; }
        
        public string Exchange { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class MarketDataHistory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal OpenPrice { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal HighPrice { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal LowPrice { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ClosePrice { get; set; }
        
        [Required]
        public long Volume { get; set; }
        
        public decimal? AdjustedClose { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(Symbol), nameof(Date), IsUnique = true)]
        public class MarketDataHistoryIndex { }
    }

    // Company Research System
    public class CompanyProfile
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public string Sector { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        
        public string? Website { get; set; }
        public string? CEO { get; set; }
        public int? Employees { get; set; }
        public DateTime? Founded { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CompanyFinancial
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        public DateTime ReportDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Period { get; set; } = string.Empty; // Q1, Q2, Q3, Q4, Annual
        
        [Required]
        public int Year { get; set; }
        
        public decimal? Revenue { get; set; }
        public decimal? NetIncome { get; set; }
        public decimal? GrossProfit { get; set; }
        public decimal? OperatingIncome { get; set; }
        public decimal? TotalAssets { get; set; }
        public decimal? TotalLiabilities { get; set; }
        public decimal? TotalEquity { get; set; }
        public decimal? CashFlow { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(Symbol), nameof(ReportDate), nameof(Period), IsUnique = true)]
        public class CompanyFinancialIndex { }
    }

    public class CompanyNews
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Headline { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? Source { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        
        [Required]
        public DateTime PublishedAt { get; set; }
        
        public string? Sentiment { get; set; } // Positive, Negative, Neutral
        public decimal? SentimentScore { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Economic Indicators System
    public class EconomicIndicator
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // GDP, Inflation, Employment, etc.
        
        [Required]
        [StringLength(20)]
        public string Frequency { get; set; } = string.Empty; // Monthly, Quarterly, Annual
        
        public string Unit { get; set; } = string.Empty; // Percentage, Dollars, Index, etc.
        public string Source { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<EconomicIndicatorHistory> History { get; set; } = new List<EconomicIndicatorHistory>();
    }

    public class EconomicIndicatorHistory
    {
        public int Id { get; set; }
        
        public int IndicatorId { get; set; }
        
        [ForeignKey("IndicatorId")]
        public virtual EconomicIndicator Indicator { get; set; } = null!;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public decimal Value { get; set; }
        
        public decimal? PreviousValue { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercent { get; set; }
        
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Composite unique constraint
        [Index(nameof(IndicatorId), nameof(Date), IsUnique = true)]
        public class EconomicIndicatorHistoryIndex { }
    }
}
