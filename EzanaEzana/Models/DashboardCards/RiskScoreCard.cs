using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EzanaEzana.Models.DashboardCards
{
    /// <summary>
    /// Model for the Risk Score dashboard card
    /// </summary>
    public class RiskScoreCard
    {
        /// <summary>
        /// Current risk score (0-10 scale)
        /// </summary>
        [Display(Name = "Risk Score")]
        [Range(0, 10)]
        public decimal Score { get; set; }

        /// <summary>
        /// Risk category classification
        /// </summary>
        [Display(Name = "Risk Category")]
        public RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// Previous risk score for comparison
        /// </summary>
        [Display(Name = "Previous Score")]
        public decimal PreviousScore { get; set; }

        /// <summary>
        /// Change in risk score from previous
        /// </summary>
        [Display(Name = "Score Change")]
        public decimal ScoreChange { get; set; }

        /// <summary>
        /// Description of the risk profile
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// When the risk score was last calculated
        /// </summary>
        [Display(Name = "Last Calculated")]
        public DateTime LastCalculated { get; set; }

        /// <summary>
        /// Individual risk factors contributing to the score
        /// </summary>
        public List<RiskFactor> RiskFactors { get; set; } = new();

        /// <summary>
        /// Portfolio volatility measure
        /// </summary>
        public decimal Volatility { get; set; }

        /// <summary>
        /// Beta relative to market
        /// </summary>
        public decimal Beta { get; set; }

        /// <summary>
        /// Sharpe ratio (risk-adjusted return)
        /// </summary>
        public decimal SharpeRatio { get; set; }

        /// <summary>
        /// Maximum drawdown percentage
        /// </summary>
        public decimal MaxDrawdown { get; set; }

        /// <summary>
        /// Formatted display values for the UI
        /// </summary>
        public RiskScoreDisplay DisplayValues => new()
        {
            ScoreFormatted = $"{Score:F1}/10",
            PreviousScoreFormatted = $"{PreviousScore:F1}/10",
            ScoreChangeFormatted = ScoreChange.ToString("F1"),
            LastCalculatedFormatted = LastCalculated.ToString("MMM dd, yyyy"),
            IsImproving = ScoreChange <= 0, // Lower score = lower risk = improving
            RiskLevel = GetRiskLevel(),
            RiskColor = GetRiskColor(),
            VolatilityFormatted = Volatility.ToString("P2"),
            BetaFormatted = Beta.ToString("F2"),
            SharpeRatioFormatted = SharpeRatio.ToString("F2")
        };

        private RiskLevel GetRiskLevel()
        {
            return Score switch
            {
                < 3 => RiskLevel.Low,
                < 5 => RiskLevel.ModerateLow,
                < 7 => RiskLevel.Moderate,
                < 9 => RiskLevel.ModerateHigh,
                _ => RiskLevel.High
            };
        }

        private string GetRiskColor()
        {
            return Score switch
            {
                < 3 => "#10B981", // Green
                < 5 => "#34D399", // Light Green
                < 7 => "#F59E0B", // Yellow
                < 9 => "#F97316", // Orange
                _ => "#EF4444"   // Red
            };
        }
    }

    /// <summary>
    /// Display-specific values for the Risk Score card
    /// </summary>
    public class RiskScoreDisplay
    {
        public string ScoreFormatted { get; set; } = string.Empty;
        public string PreviousScoreFormatted { get; set; } = string.Empty;
        public string ScoreChangeFormatted { get; set; } = string.Empty;
        public string LastCalculatedFormatted { get; set; } = string.Empty;
        public bool IsImproving { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public string RiskColor { get; set; } = string.Empty;
        public string VolatilityFormatted { get; set; } = string.Empty;
        public string BetaFormatted { get; set; } = string.Empty;
        public string SharpeRatioFormatted { get; set; } = string.Empty;
    }

    /// <summary>
    /// Risk category classification
    /// </summary>
    public enum RiskCategory
    {
        [Display(Name = "Very Low")]
        VeryLow,
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Moderate-Low")]
        ModerateLow,
        [Display(Name = "Moderate")]
        Moderate,
        [Display(Name = "Moderate-High")]
        ModerateHigh,
        [Display(Name = "High")]
        High,
        [Display(Name = "Very High")]
        VeryHigh
    }

    /// <summary>
    /// Risk level for UI display
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
