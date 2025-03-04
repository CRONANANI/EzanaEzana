using Ezana.Models;
using System.Collections.Generic;

namespace Ezana.ViewModels
{
    public class GRPVViewModel
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public List<GRPVModel> UserGRPVModels { get; set; } = new List<GRPVModel>();
        public GRPVModel? SelectedModel { get; set; }
        public Dictionary<string, string> AvailableStocks { get; set; } = new Dictionary<string, string>();
        public string? SearchQuery { get; set; }
    }
    
    public class GRPVDetailViewModel
    {
        public GRPVModel Model { get; set; } = null!;
        
        // Helper properties for displaying risk tolerance
        public string RiskToleranceCategory => GetRiskToleranceCategory(Model.RiskScore);
        public string GrowthCategory => GetScoreCategory(Model.GrowthScore);
        public string ProfitabilityCategory => GetScoreCategory(Model.ProfitabilityScore);
        public string ValuationCategory => GetScoreCategory(Model.ValuationScore);
        public string OverallCategory => GetScoreCategory(Model.OverallScore);
        
        private string GetRiskToleranceCategory(decimal score)
        {
            if (score >= 80) return "Very Conservative";
            if (score >= 60) return "Conservative";
            if (score >= 40) return "Moderate";
            if (score >= 20) return "Aggressive";
            return "Very Aggressive";
        }
        
        private string GetScoreCategory(decimal score)
        {
            if (score >= 80) return "Excellent";
            if (score >= 60) return "Good";
            if (score >= 40) return "Average";
            if (score >= 20) return "Below Average";
            return "Poor";
        }
    }
} 