using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzanaEzana.Models
{
    // Public Officials Data Models
    public class CongressTrade
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderRole { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Chamber { get; set; } = string.Empty;
        [StringLength(50)]
        public string? Party { get; set; }
        [StringLength(50)]
        public string? State { get; set; }
        [Required]
        [StringLength(20)]
        public string TradeType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime DisclosureDate { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }
        public bool IsHighPriority { get; set; }
        public string? ConflictPriority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class GovernmentContractItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string ContractNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string ContractTitle { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string ContractorName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string AgencyName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string ContractType { get; set; } = string.Empty;
        public decimal ContractValue { get; set; }
        public DateTime AwardDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(100)]
        public string? Industry { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public bool IsHighValue { get; set; }
        public string? ContractStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class HouseTrade
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderRole { get; set; } = string.Empty;
        [StringLength(50)]
        public string? Party { get; set; }
        [StringLength(50)]
        public string? State { get; set; }
        [StringLength(50)]
        public string? District { get; set; }
        [Required]
        [StringLength(20)]
        public string TradeType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime DisclosureDate { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }
        public bool IsHighPriority { get; set; }
        public string? ConflictPriority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class LobbyingActivityItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string ReportNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string LobbyingFirm { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string IssueArea { get; set; } = string.Empty;
        public decimal AmountSpent { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityEndDate { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }
        [StringLength(100)]
        public string? Industry { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public bool IsHighSpending { get; set; }
        public string? CompliancePriority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SenatorTrade
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string TraderRole { get; set; } = string.Empty;
        [StringLength(50)]
        public string? Party { get; set; }
        [StringLength(50)]
        public string? State { get; set; }
        [StringLength(50)]
        public string? Committee { get; set; }
        [Required]
        [StringLength(20)]
        public string TradeType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Value { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime DisclosureDate { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }
        public bool IsHighPriority { get; set; }
        public string? ConflictPriority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PatentItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string PatentNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        [StringLength(200)]
        public string? InventorName { get; set; }
        [Required]
        [StringLength(100)]
        public string TechnologyCategory { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Industry { get; set; }
        public DateTime FilingDate { get; set; }
        public DateTime? GrantDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }
        [StringLength(500)]
        public string? Abstract { get; set; }
        public bool IsHighQuality { get; set; }
        public decimal? QualityScore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class MarketIndicator
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string IndicatorName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string IndicatorType { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal? PreviousValue { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercent { get; set; }
        public DateTime Date { get; set; }
        [StringLength(50)]
        public string? Timeframe { get; set; }
        [StringLength(100)]
        public string? Source { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public bool IsSignificant { get; set; }
        public string? SentimentType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
