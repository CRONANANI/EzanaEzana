using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ezana.Models
{
    public class InvestmentPreference
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [Range(1, 10)]
        public int RiskTolerance { get; set; } = 5; // 1-10 scale, 1 being most conservative
        
        [Range(0, 100)]
        public int StockAllocation { get; set; } = 60; // Percentage
        
        [Range(0, 100)]
        public int BondAllocation { get; set; } = 30; // Percentage
        
        [Range(0, 100)]
        public int CashAllocation { get; set; } = 10; // Percentage
        
        [Range(0, 100)]
        public int AlternativeAllocation { get; set; } = 0; // Percentage
        
        [MaxLength(50)]
        public string InvestmentHorizon { get; set; } = "Long-term"; // Short-term, Medium-term, Long-term
        
        [MaxLength(255)]
        public string? InvestmentGoals { get; set; } // Retirement, Education, Home Purchase, etc.
        
        public bool ESGFocus { get; set; } = false; // Environmental, Social, Governance focus
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
} 