using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzanaEzana.Models
{
    // GRPV (Growth, Risk, Profitability, Valuation) Analysis System
    public class GRPVModel
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        
        // Growth Factors
        public Dictionary<string, decimal> GrowthFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Risk Factors
        public Dictionary<string, decimal> RiskFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Profitability Factors
        public Dictionary<string, decimal> ProfitabilityFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Valuation Factors
        public Dictionary<string, decimal> ValuationFactors { get; set; } = new Dictionary<string, decimal>();
        
        // Overall Scores
        public decimal GrowthScore { get; set; }
        public decimal RiskScore { get; set; }
        public decimal ProfitabilityScore { get; set; }
        public decimal ValuationScore { get; set; }
        public decimal OverallScore { get; set; }
        
        // Analysis Details
        [StringLength(1000)]
        public string? GrowthAnalysis { get; set; }
        
        [StringLength(1000)]
        public string? RiskAnalysis { get; set; }
        
        [StringLength(1000)]
        public string? ProfitabilityAnalysis { get; set; }
        
        [StringLength(1000)]
        public string? ValuationAnalysis { get; set; }
        
        [StringLength(1000)]
        public string? OverallAnalysis { get; set; }
        
        // Recommendations
        [StringLength(50)]
        public string Recommendation { get; set; } = string.Empty; // Buy, Hold, Sell
        
        [StringLength(500)]
        public string? RecommendationReason { get; set; }
        
        public decimal? TargetPrice { get; set; }
        public decimal? StopLossPrice { get; set; }
        
        // Timestamps
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
