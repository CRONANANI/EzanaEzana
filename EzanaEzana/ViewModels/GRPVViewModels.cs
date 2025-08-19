using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // GRPV ViewModels
    public class GRPVViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public List<GRPVDetailViewModel> UserGRPVModels { get; set; } = new List<GRPVDetailViewModel>();
        public List<string> AvailableStocks { get; set; } = new List<string>();
        public string? SearchQuery { get; set; }
    }

    public class GRPVDetailViewModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public decimal GrowthScore { get; set; }
        public decimal RiskScore { get; set; }
        public decimal ProfitabilityScore { get; set; }
        public decimal ValuationScore { get; set; }
        public decimal OverallScore { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public string? RecommendationReason { get; set; }
        public decimal? TargetPrice { get; set; }
        public decimal? StopLossPrice { get; set; }
        public DateTime AnalysisDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Growth Analysis
        public Dictionary<string, decimal> GrowthFactors { get; set; } = new Dictionary<string, decimal>();
        public string? GrowthAnalysis { get; set; }
        
        // Risk Analysis
        public Dictionary<string, decimal> RiskFactors { get; set; } = new Dictionary<string, decimal>();
        public string? RiskAnalysis { get; set; }
        
        // Profitability Analysis
        public Dictionary<string, decimal> ProfitabilityFactors { get; set; } = new Dictionary<string, decimal>();
        public string? ProfitabilityAnalysis { get; set; }
        
        // Valuation Analysis
        public Dictionary<string, decimal> ValuationFactors { get; set; } = new Dictionary<string, decimal>();
        public string? ValuationAnalysis { get; set; }
        
        // Overall Analysis
        public string? OverallAnalysis { get; set; }
        
        // Computed Properties
        public string GrowthScoreColor => GetScoreColor(GrowthScore);
        public string RiskScoreColor => GetScoreColor(RiskScore);
        public string ProfitabilityScoreColor => GetScoreColor(ProfitabilityScore);
        public string ValuationScoreColor => GetScoreColor(ValuationScore);
        public string OverallScoreColor => GetScoreColor(OverallScore);
        public string RecommendationColor => GetRecommendationColor(Recommendation);
        public bool HasTargetPrice => TargetPrice.HasValue;
        public bool HasStopLoss => StopLossPrice.HasValue;
        public bool HasGrowthAnalysis => !string.IsNullOrEmpty(GrowthAnalysis);
        public bool HasRiskAnalysis => !string.IsNullOrEmpty(RiskAnalysis);
        public bool HasProfitabilityAnalysis => !string.IsNullOrEmpty(ProfitabilityAnalysis);
        public bool HasValuationAnalysis => !string.IsNullOrEmpty(ValuationAnalysis);
        public bool HasOverallAnalysis => !string.IsNullOrEmpty(OverallAnalysis);
        public bool HasRecommendationReason => !string.IsNullOrEmpty(RecommendationReason);
        
        // Factor Analysis
        public List<FactorViewModel> TopGrowthFactors => GetTopFactors(GrowthFactors, 5);
        public List<FactorViewModel> TopRiskFactors => GetTopFactors(RiskFactors, 5);
        public List<FactorViewModel> TopProfitabilityFactors => GetTopFactors(ProfitabilityFactors, 5);
        public List<FactorViewModel> TopValuationFactors => GetTopFactors(ValuationFactors, 5);
        
        private string GetScoreColor(decimal score)
        {
            return score switch
            {
                >= 8.0m => "text-green-600",
                >= 6.0m => "text-blue-600",
                >= 4.0m => "text-yellow-600",
                >= 2.0m => "text-orange-600",
                _ => "text-red-600"
            };
        }
        
        private string GetRecommendationColor(string recommendation)
        {
            return recommendation.ToLower() switch
            {
                "strong buy" => "text-green-600",
                "buy" => "text-blue-600",
                "hold" => "text-yellow-600",
                "sell" => "text-orange-600",
                "strong sell" => "text-red-600",
                _ => "text-gray-600"
            };
        }
        
        private List<FactorViewModel> GetTopFactors(Dictionary<string, decimal> factors, int count)
        {
            return factors
                .OrderByDescending(f => f.Value)
                .Take(count)
                .Select(f => new FactorViewModel { Name = f.Key, Value = f.Value })
                .ToList();
        }
    }

    public class FactorViewModel
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        
        public string Color => Value switch
        {
            >= 8.0m => "text-green-600",
            >= 6.0m => "text-blue-600",
            >= 4.0m => "text-yellow-600",
            >= 2.0m => "text-orange-600",
            _ => "text-red-600"
        };
    }

    public class CreateGRPVViewModel
    {
        [Required(ErrorMessage = "Symbol is required")]
        [StringLength(10, ErrorMessage = "Symbol cannot be longer than 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "Company name cannot be longer than 100 characters")]
        public string? CompanyName { get; set; }
        
        [StringLength(100, ErrorMessage = "Sector cannot be longer than 100 characters")]
        public string? Sector { get; set; }
        
        [StringLength(100, ErrorMessage = "Industry cannot be longer than 100 characters")]
        public string? Industry { get; set; }
        
        [Range(0, 10, ErrorMessage = "Growth score must be between 0 and 10")]
        public decimal GrowthScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Risk score must be between 0 and 10")]
        public decimal RiskScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Profitability score must be between 0 and 10")]
        public decimal ProfitabilityScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Valuation score must be between 0 and 10")]
        public decimal ValuationScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Overall score must be between 0 and 10")]
        public decimal OverallScore { get; set; }
        
        [Required(ErrorMessage = "Recommendation is required")]
        [StringLength(50, ErrorMessage = "Recommendation cannot be longer than 50 characters")]
        public string Recommendation { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Recommendation reason cannot be longer than 1000 characters")]
        public string? RecommendationReason { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Target price must be greater than 0")]
        public decimal? TargetPrice { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Stop loss price must be greater than 0")]
        public decimal? StopLossPrice { get; set; }
        
        [StringLength(1000, ErrorMessage = "Growth analysis cannot be longer than 1000 characters")]
        public string? GrowthAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Risk analysis cannot be longer than 1000 characters")]
        public string? RiskAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Profitability analysis cannot be longer than 1000 characters")]
        public string? ProfitabilityAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Valuation analysis cannot be longer than 1000 characters")]
        public string? ValuationAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Overall analysis cannot be longer than 1000 characters")]
        public string? OverallAnalysis { get; set; }
    }

    public class UpdateGRPVViewModel
    {
        [Range(0, 10, ErrorMessage = "Growth score must be between 0 and 10")]
        public decimal GrowthScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Risk score must be between 0 and 10")]
        public decimal RiskScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Profitability score must be between 0 and 10")]
        public decimal ProfitabilityScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Valuation score must be between 0 and 10")]
        public decimal ValuationScore { get; set; }
        
        [Range(0, 10, ErrorMessage = "Overall score must be between 0 and 10")]
        public decimal OverallScore { get; set; }
        
        [Required(ErrorMessage = "Recommendation is required")]
        [StringLength(50, ErrorMessage = "Recommendation cannot be longer than 50 characters")]
        public string Recommendation { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Recommendation reason cannot be longer than 1000 characters")]
        public string? RecommendationReason { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Target price must be greater than 0")]
        public decimal? TargetPrice { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Stop loss price must be greater than 0")]
        public decimal? StopLossPrice { get; set; }
        
        [StringLength(1000, ErrorMessage = "Growth analysis cannot be longer than 1000 characters")]
        public string? GrowthAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Risk analysis cannot be longer than 1000 characters")]
        public string? RiskAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Profitability analysis cannot be longer than 1000 characters")]
        public string? ProfitabilityAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Valuation analysis cannot be longer than 1000 characters")]
        public string? ValuationAnalysis { get; set; }
        
        [StringLength(1000, ErrorMessage = "Overall analysis cannot be longer than 1000 characters")]
        public string? OverallAnalysis { get; set; }
    }

    public class GRPVSearchViewModel
    {
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public string? Recommendation { get; set; }
        public decimal? MinOverallScore { get; set; }
        public decimal? MaxOverallScore { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "AnalysisDate";
        public string SortOrder { get; set; } = "Desc";
        
        public List<GRPVDetailViewModel> Results { get; set; } = new List<GRPVDetailViewModel>();
        public int TotalResults { get; set; }
        public bool HasMoreResults { get; set; }
    }
}
