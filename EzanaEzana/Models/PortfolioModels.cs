using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzanaEzana.Models
{
    // Portfolio System
    public class Portfolio
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<PortfolioHolding> Holdings { get; set; } = new List<PortfolioHolding>();
        public virtual ICollection<PortfolioTransaction> Transactions { get; set; } = new List<PortfolioTransaction>();
    }

    public class PortfolioHolding
    {
        public int Id { get; set; }
        
        public int PortfolioId { get; set; }
        
        [ForeignKey("PortfolioId")]
        public virtual Portfolio Portfolio { get; set; } = null!;
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Shares { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal AveragePrice { get; set; }
        
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PortfolioTransaction
    {
        public int Id { get; set; }
        
        public int PortfolioId { get; set; }
        
        [ForeignKey("PortfolioId")]
        public virtual Portfolio Portfolio { get; set; } = null!;
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; } = string.Empty; // Buy, Sell, Dividend, Split
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Shares { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        public decimal? Fees { get; set; }
        
        [Required]
        public DateTime TransactionDate { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    // Watchlist System
    public class Watchlist
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        public bool IsDefault { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<WatchlistItem> Items { get; set; } = new List<WatchlistItem>();
    }

    public class WatchlistItem
    {
        public int Id { get; set; }
        
        public int WatchlistId { get; set; }
        
        [ForeignKey("WatchlistId")]
        public virtual Watchlist Watchlist { get; set; } = null!;
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        public decimal? TargetPrice { get; set; }
        public decimal? StopLossPrice { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
