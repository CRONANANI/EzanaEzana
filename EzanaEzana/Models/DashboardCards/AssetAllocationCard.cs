using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Asset Allocation dashboard card
    /// </summary>
    public class AssetAllocationCard
    {
        /// <summary>
        /// Type of breakdown (asset class, sector, performance, etc.)
        /// </summary>
        [Display(Name = "Breakdown Type")]
        public string BreakdownType { get; set; } = "asset_class";

        /// <summary>
        /// Allocation items with percentages and values
        /// </summary>
        public List<AssetAllocationItem> Items { get; set; } = new();

        /// <summary>
        /// Total portfolio value
        /// </summary>
        public decimal TotalPortfolioValue { get; set; }

        /// <summary>
        /// Diversification score (0-100)
        /// </summary>
        [Range(0, 100)]
        public decimal DiversificationScore { get; set; }

        /// <summary>
        /// Risk assessment based on allocation
        /// </summary>
        public string RiskAssessment { get; set; } = string.Empty;

        /// <summary>
        /// Rebalancing recommendations
        /// </summary>
        public List<RebalancingRecommendation> RebalancingRecommendations { get; set; } = new();

        /// <summary>
        /// Last rebalancing date
        /// </summary>
        public DateTime? LastRebalancingDate { get; set; }

        /// <summary>
        /// Target allocation percentages
        /// </summary>
        public Dictionary<string, decimal> TargetAllocations { get; set; } = new();

        /// <summary>
        /// Deviation from target allocations
        /// </summary>
        public Dictionary<string, decimal> AllocationDeviations { get; set; } = new();

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public AssetAllocationDisplay DisplayValues => new()
        {
            DiversificationScoreFormatted = $"{DiversificationScore:F0}/100",
            TotalValueFormatted = TotalPortfolioValue.ToString("C"),
            RiskLevel = GetRiskLevel(),
            RiskColor = GetRiskColor(),
            NeedsRebalancing = AllocationDeviations.Values.Any(d => Math.Abs(d) > 5.0m),
            LargestDeviation = AllocationDeviations.Values.Any() ? AllocationDeviations.Values.Max(Math.Abs) : 0,
            LargestDeviationFormatted = AllocationDeviations.Values.Any() ? $"{AllocationDeviations.Values.Max(Math.Abs):F1}%" : "0%"
        };

        private RiskLevel GetRiskLevel()
        {
            return DiversificationScore switch
            {
                >= 80 => RiskLevel.Low,
                >= 60 => RiskLevel.ModerateLow,
                >= 40 => RiskLevel.Moderate,
                >= 20 => RiskLevel.ModerateHigh,
                _ => RiskLevel.High
            };
        }

        private string GetRiskColor()
        {
            return DiversificationScore switch
            {
                >= 80 => "#10B981", // Green
                >= 60 => "#34D399", // Light Green
                >= 40 => "#F59E0B", // Yellow
                >= 20 => "#F97316", // Orange
                _ => "#EF4444"      // Red
            };
        }
    }

    /// <summary>
    /// Display-specific values for the Asset Allocation card
    /// </summary>
    public class AssetAllocationDisplay
    {
        public string DiversificationScoreFormatted { get; set; } = string.Empty;
        public string TotalValueFormatted { get; set; } = string.Empty;
        public RiskLevel RiskLevel { get; set; }
        public string RiskColor { get; set; } = string.Empty;
        public bool NeedsRebalancing { get; set; }
        public decimal LargestDeviation { get; set; }
        public string LargestDeviationFormatted { get; set; } = string.Empty;
    }

    /// <summary>
    /// Individual asset allocation item
    /// </summary>
    public class AssetAllocationItem
    {
        /// <summary>
        /// Label/name of the allocation category
        /// </summary>
        [Display(Name = "Label")]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Dollar value allocated
        /// </summary>
        [Display(Name = "Value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Percentage of total portfolio
        /// </summary>
        [Display(Name = "Percentage")]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Color for chart display
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Target percentage for this category
        /// </summary>
        public decimal TargetPercentage { get; set; }

        /// <summary>
        /// Deviation from target percentage
        /// </summary>
        public decimal DeviationFromTarget { get; set; }

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string ValueFormatted => Value.ToString("C");
        public string PercentageFormatted => $"{Percentage:F1}%";
        public string TargetPercentageFormatted => $"{TargetPercentage:F1}%";
        public string DeviationFormatted => $"{DeviationFromTarget:F1}%";
        public bool IsOverAllocated => DeviationFromTarget > 0;
        public bool IsUnderAllocated => DeviationFromTarget < 0;
    }

    /// <summary>
    /// Rebalancing recommendation
    /// </summary>
    public class RebalancingRecommendation
    {
        /// <summary>
        /// Asset category to rebalance
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Action to take (buy/sell)
        /// </summary>
        public RebalancingAction Action { get; set; }

        /// <summary>
        /// Amount to adjust
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// Priority level of the recommendation
        /// </summary>
        public RecommendationPriority Priority { get; set; }

        /// <summary>
        /// Reason for the recommendation
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Formatted display values
        /// </summary>
        public string AdjustmentAmountFormatted => AdjustmentAmount.ToString("C");
        public string PriorityFormatted => Priority.ToString().Replace("_", " ");
    }

    /// <summary>
    /// Rebalancing action types
    /// </summary>
    public enum RebalancingAction
    {
        [Display(Name = "Buy")]
        Buy,
        [Display(Name = "Sell")]
        Sell,
        [Display(Name = "Hold")]
        Hold
    }

    /// <summary>
    /// Recommendation priority levels
    /// </summary>
    public enum RecommendationPriority
    {
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "High")]
        High,
        [Display(Name = "Critical")]
        Critical
    }

    /// <summary>
    /// Risk level for asset allocation
    /// </summary>
    public enum RiskLevel
    {
        [Display(Name = "Low Risk")]
        Low,
        [Display(Name = "Moderate-Low Risk")]
        ModerateLow,
        [Display(Name = "Moderate Risk")]
        Moderate,
        [Display(Name = "Moderate-High Risk")]
        ModerateHigh,
        [Display(Name = "High Risk")]
        High
    }
}
