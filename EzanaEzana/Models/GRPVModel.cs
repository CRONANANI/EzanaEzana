using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ezana.Models
{
    public class GRPVModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        public string StockSymbol { get; set; } = null!;
        
        [Required]
        public string CompanyName { get; set; } = null!;
        
        // Growth Score (0-100)
        public decimal GrowthScore { get; set; }
        
        // Risk Score (0-100)
        public decimal RiskScore { get; set; }
        
        // Profitability Score (0-100)
        public decimal ProfitabilityScore { get; set; }
        
        // Valuation Score (0-100)
        public decimal ValuationScore { get; set; }
        
        // Overall GRPV Score (0-100)
        public decimal OverallScore { get; set; }
        
        // Growth Factors
        public Dictionary<string, decimal> GrowthFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Risk Factors
        public Dictionary<string, decimal> RiskFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Profitability Factors
        public Dictionary<string, decimal> ProfitabilityFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Valuation Factors
        public Dictionary<string, decimal> ValuationFactors { get; set; } = new Dictionary<string, decimal>();
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
} 