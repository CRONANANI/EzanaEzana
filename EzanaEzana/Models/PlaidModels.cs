using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzanaEzana.Models
{
    // Plaid Integration Models
    public class PlaidItem
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string PlaidItemId { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string PlaidAccessToken { get; set; } = string.Empty;
        [StringLength(100)]
        public string? PlaidInstitutionId { get; set; }
        [StringLength(100)]
        public string? PlaidRequestId { get; set; }
        public string? WebhookUrl { get; set; }
        public string? Error { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ICollection<PlaidAccount> Accounts { get; set; } = new List<PlaidAccount>();
        public virtual PlaidInstitution? Institution { get; set; }
    }

    public class PlaidAccount
    {
        public int Id { get; set; }
        [Required]
        public int PlaidItemId { get; set; }
        [ForeignKey("PlaidItemId")]
        public virtual PlaidItem PlaidItem { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string PlaidAccountId { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Mask { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? OfficialName { get; set; }
        [StringLength(50)]
        public string? Type { get; set; }
        [StringLength(50)]
        public string? Subtype { get; set; }
        public decimal? CurrentBalance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public string? CurrencyCode { get; set; } = "USD";
        public bool IsActive { get; set; } = true;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual ICollection<PlaidTransaction> Transactions { get; set; } = new List<PlaidTransaction>();
    }

    public class PlaidTransaction
    {
        public int Id { get; set; }
        [Required]
        public int PlaidAccountId { get; set; }
        [ForeignKey("PlaidAccountId")]
        public virtual PlaidAccount PlaidAccount { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string PlaidTransactionId { get; set; } = string.Empty;
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(200)]
        public string? MerchantName { get; set; }
        [StringLength(100)]
        public string? Category { get; set; }
        [StringLength(100)]
        public string? CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string? CurrencyCode { get; set; } = "USD";
        public DateTime Date { get; set; }
        public DateTime? AuthorizedDate { get; set; }
        public DateTime? SettledDate { get; set; }
        [StringLength(100)]
        public string? PaymentChannel { get; set; }
        public bool Pending { get; set; }
        [StringLength(100)]
        public string? PaymentMethod { get; set; }
        [StringLength(100)]
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PlaidInstitution
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string PlaidInstitutionId { get; set; } = string.Empty;
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(200)]
        public string? DisplayName { get; set; }
        [StringLength(100)]
        public string? Type { get; set; }
        [StringLength(100)]
        public string? CountryCode { get; set; }
        public string? Logo { get; set; }
        public string? PrimaryColor { get; set; }
        public string? Url { get; set; }
        public string? PrimaryBrandColor { get; set; }
        public string? RoutingNumbers { get; set; }
        public string? OcrNumbers { get; set; }
        public string? LogoUrl { get; set; }
        public string? PrimaryBrandLogoUrl { get; set; }
        public bool HasMfa { get; set; }
        public string? MfaType { get; set; }
        public string? Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual ICollection<PlaidItem> PlaidItems { get; set; } = new List<PlaidItem>();
    }
}
