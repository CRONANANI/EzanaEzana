using System.ComponentModel.DataAnnotations;

namespace Ezana.Models.DashboardCards
{
    /// <summary>
    /// Individual risk factor contributing to the overall risk score
    /// </summary>
    public class RiskFactor
    {
        /// <summary>
        /// Name of the risk factor
        /// </summary>
        [Display(Name = "Risk Factor")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Weight of this factor in the overall risk calculation (0-1)
        /// </summary>
        [Display(Name = "Weight")]
        [Range(0, 1)]
        public decimal Weight { get; set; }

        /// <summary>
        /// Risk value for this factor (0-10 scale)
        /// </summary>
        [Display(Name = "Risk Value")]
        [Range(0, 10)]
        public decimal Value { get; set; }

        /// <summary>
        /// Description of the risk factor
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Formatted weight as percentage
        /// </summary>
        public string WeightFormatted => $"{Weight:P0}";

        /// <summary>
        /// Formatted risk value
        /// </summary>
        public string ValueFormatted => $"{Value:F1}/10";

        /// <summary>
        /// Risk level for this factor
        /// </summary>
        public RiskLevel RiskLevel => Value switch
        {
            < 3 => RiskLevel.Low,
            < 5 => RiskLevel.ModerateLow,
            < 7 => RiskLevel.Moderate,
            < 9 => RiskLevel.ModerateHigh,
            _ => RiskLevel.High
        };
    }
}
