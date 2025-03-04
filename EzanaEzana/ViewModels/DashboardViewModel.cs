using System.Collections.Generic;

namespace Ezana.ViewModels
{
    public class DashboardViewModel
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public Dictionary<string, object> InvestmentRecommendations { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> PortfolioAllocation { get; set; } = new Dictionary<string, object>();
    }

    public class InvestmentPreferenceViewModel
    {
        public int RiskTolerance { get; set; } = 5;
        public int StockAllocation { get; set; } = 60;
        public int BondAllocation { get; set; } = 30;
        public int CashAllocation { get; set; } = 10;
        public int AlternativeAllocation { get; set; } = 0;
        public string InvestmentHorizon { get; set; } = "Long-term";
        public string? InvestmentGoals { get; set; }
        public bool ESGFocus { get; set; } = false;
    }
} 