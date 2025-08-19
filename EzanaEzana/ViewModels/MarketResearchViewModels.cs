using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Market Summary ViewModels
    public class MarketSummaryViewModel
    {
        public int TotalStocks { get; set; }
        public int Gainers { get; set; }
        public int Losers { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal AverageChange { get; set; }
    }

    public class MarketTrendViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PriceChange { get; set; }
        public decimal PriceChangePercent { get; set; }
        public string Trend { get; set; } = string.Empty;
        public decimal Volatility { get; set; }
    }

    // Company Analysis ViewModels
    public class CompanyAnalysisViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public decimal MarketCap { get; set; }
        public decimal PE { get; set; }
        public decimal PB { get; set; }
        public decimal DividendYield { get; set; }
    }

    public class CompanyMetricsViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal NetIncome { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal OperatingMargin { get; set; }
        public decimal NetMargin { get; set; }
        public decimal ROE { get; set; }
        public decimal ROA { get; set; }
    }

    public class CompanyValuationViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal IntrinsicValue { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal UpsidePotential { get; set; }
        public string ValuationMethod { get; set; } = string.Empty;
        public decimal DiscountRate { get; set; }
    }

    public class CompanyRiskViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Beta { get; set; }
        public decimal Volatility { get; set; }
        public decimal SharpeRatio { get; set; }
        public decimal MaxDrawdown { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
    }

    public class CompanyComparisonViewModel
    {
        public List<string> Symbols { get; set; } = new List<string>();
        public List<CompanyAnalysisViewModel> Companies { get; set; } = new List<CompanyAnalysisViewModel>();
        public Dictionary<string, decimal> ComparisonMetrics { get; set; } = new Dictionary<string, decimal>();
    }

    // Economic ViewModels
    public class EconomicSummaryViewModel
    {
        public int TotalIndicators { get; set; }
        public int PositiveTrends { get; set; }
        public int NegativeTrends { get; set; }
        public decimal AverageGrowth { get; set; }
        public string OverallOutlook { get; set; } = string.Empty;
    }

    public class EconomicTrendViewModel
    {
        public string IndicatorName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal CurrentValue { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public string Trend { get; set; } = string.Empty;
    }

    public class EconomicForecastViewModel
    {
        public string IndicatorName { get; set; } = string.Empty;
        public decimal CurrentValue { get; set; }
        public decimal ForecastValue { get; set; }
        public decimal Confidence { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Factors { get; set; } = string.Empty;
    }

    // Market Intelligence ViewModels
    public class MarketInsightViewModel
    {
        public List<string> TopGainers { get; set; } = new List<string>();
        public List<string> TopLosers { get; set; } = new List<string>();
        public List<string> HighVolume { get; set; } = new List<string>();
        public List<string> LowVolume { get; set; } = new List<string>();
        public string MarketSentiment { get; set; } = string.Empty;
        public string KeyDrivers { get; set; } = string.Empty;
    }

    public class MarketSentimentViewModel
    {
        public decimal BullishPercentage { get; set; }
        public decimal BearishPercentage { get; set; }
        public decimal NeutralPercentage { get; set; }
        public string OverallSentiment { get; set; } = string.Empty;
        public List<SentimentFactorViewModel> Factors { get; set; } = new List<SentimentFactorViewModel>();
    }

    public class SentimentFactorViewModel
    {
        public string Factor { get; set; } = string.Empty;
        public decimal Impact { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class SentimentTrendViewModel
    {
        public DateTime Date { get; set; }
        public decimal BullishPercentage { get; set; }
        public decimal BearishPercentage { get; set; }
        public decimal NeutralPercentage { get; set; }
        public string Trend { get; set; } = string.Empty;
    }

    // Sector and Industry ViewModels
    public class SectorPerformanceViewModel
    {
        public string Sector { get; set; } = string.Empty;
        public decimal Performance { get; set; }
        public decimal Weight { get; set; }
        public int Constituents { get; set; }
        public string Trend { get; set; } = string.Empty;
    }

    public class IndustryTrendViewModel
    {
        public string Industry { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public decimal Performance { get; set; }
        public decimal Volatility { get; set; }
        public string Trend { get; set; } = string.Empty;
        public List<string> TopPerformers { get; set; } = new List<string>();
    }

    // Market Intelligence ViewModels (from IMarketResearchService)
    public class MarketIntelligenceViewModel
    {
        public List<string> TopGainers { get; set; } = new List<string>();
        public List<string> TopLosers { get; set; } = new List<string>();
        public List<string> HighVolume { get; set; } = new List<string>();
        public string MarketSentiment { get; set; } = string.Empty;
        public string KeyDrivers { get; set; } = string.Empty;
    }

    public class EconomicOutlookViewModel
    {
        public string OverallOutlook { get; set; } = string.Empty;
        public List<string> PositiveFactors { get; set; } = new List<string>();
        public List<string> NegativeFactors { get; set; } = new List<string>();
        public string RiskLevel { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }

    public class MarketTrendAnalysisViewModel
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PriceChange { get; set; }
        public decimal PriceChangePercent { get; set; }
        public string Trend { get; set; } = string.Empty;
        public decimal Volatility { get; set; }
        public decimal SMA20 { get; set; }
        public decimal SMA50 { get; set; }
        public decimal SMA200 { get; set; }
        public string Support { get; set; } = string.Empty;
        public string Resistance { get; set; } = string.Empty;
    }
}
