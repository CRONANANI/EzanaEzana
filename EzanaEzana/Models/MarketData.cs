using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ezana.Models
{
    public class MarketData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal HighPrice { get; set; }

        public decimal LowPrice { get; set; }

        public decimal PreviousClose { get; set; }

        public long Volume { get; set; }

        public decimal MarketCap { get; set; }

        public decimal? PE_Ratio { get; set; }

        public decimal? DividendYield { get; set; }

        public decimal? Beta { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<MarketDataHistory> PriceHistory { get; set; }

        public virtual ICollection<CompanyProfile> CompanyProfiles { get; set; }
    }

    public class MarketDataHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MarketDataId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal HighPrice { get; set; }

        public decimal LowPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public long Volume { get; set; }

        public decimal? AdjustedClose { get; set; }

        // Navigation properties
        [ForeignKey("MarketDataId")]
        public virtual MarketData MarketData { get; set; }
    }

    public class CompanyProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Sector { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string Exchange { get; set; }

        public int? Employees { get; set; }

        public DateTime? Founded { get; set; }

        [StringLength(500)]
        public string Website { get; set; }

        [StringLength(500)]
        public string LogoUrl { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<CompanyFinancial> Financials { get; set; }

        public virtual ICollection<CompanyNews> News { get; set; }
    }

    public class CompanyFinancial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyProfileId { get; set; }

        [Required]
        [StringLength(50)]
        public string Period { get; set; } // Annual, Quarterly

        public int Year { get; set; }

        public int? Quarter { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? NetIncome { get; set; }

        public decimal? TotalAssets { get; set; }

        public decimal? TotalLiabilities { get; set; }

        public decimal? CashFlow { get; set; }

        public decimal? Debt { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("CompanyProfileId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    }

    public class CompanyNews
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyProfileId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Summary { get; set; }

        [StringLength(500)]
        public string Source { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("CompanyProfileId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    }

    public class EconomicIndicator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } // GDP, Inflation, Employment, etc.

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } // Percentage, Dollars, Index, etc.

        public decimal CurrentValue { get; set; }

        public decimal? PreviousValue { get; set; }

        public decimal? Change { get; set; }

        public decimal? ChangePercent { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<EconomicIndicatorHistory> History { get; set; }
    }

    public class EconomicIndicatorHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EconomicIndicatorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal Value { get; set; }

        public decimal? Change { get; set; }

        public decimal? ChangePercent { get; set; }

        // Navigation properties
        [ForeignKey("EconomicIndicatorId")]
        public virtual EconomicIndicator EconomicIndicator { get; set; }
    }
}
