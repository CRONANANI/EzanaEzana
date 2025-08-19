using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EzanaEzana.Models.PublicOfficials
{
    public class MarketSentimentCard
    {
        // Core Data Properties
        public string OverallSentiment { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public DateTime LastUpdated { get; set; }
        public int TotalIndicators { get; set; }
        public int BullishIndicators { get; set; }
        public int BearishIndicators { get; set; }
        public int NeutralIndicators { get; set; }

        // Sentiment Metrics
        public decimal BullishPercentage { get; set; }
        public decimal BearishPercentage { get; set; }
        public decimal NeutralPercentage { get; set; }
        public decimal SentimentChange { get; set; }
        public decimal PreviousSentimentScore { get; set; }

        // Time-based Analysis
        public List<DailySentimentData> DailyData { get; set; } = new List<DailySentimentData>();
        public List<WeeklySentimentData> WeeklyData { get; set; } = new List<WeeklySentimentData>();
        public List<MonthlySentimentData> MonthlyData { get; set; } = new List<MonthlySentimentData>();

        // Market Indicators
        public List<MarketIndicator> MarketIndicators { get; set; } = new List<MarketIndicator>();
        public List<SentimentIndicator> SentimentIndicators { get; set; } = new List<SentimentIndicator>();
        public List<TechnicalIndicator> TechnicalIndicators { get; set; } = new List<TechnicalIndicator>();

        // Sector Sentiment
        public List<SectorSentiment> SectorData { get; set; } = new List<SectorSentiment>();
        public List<IndustrySentiment> IndustryData { get; set; } = new List<IndustrySentiment>();

        // Geographic Sentiment
        public List<RegionalSentiment> RegionalData { get; set; } = new List<RegionalSentiment>();
        public List<CountrySentiment> CountryData { get; set; } = new List<CountrySentiment>();

        // Market Events and News
        public List<MarketEvent> RecentEvents { get; set; } = new List<MarketEvent>();
        public List<NewsSentiment> NewsSentiment { get; set; } = new List<NewsSentiment>();
        public List<EarningsSentiment> EarningsSentiment { get; set; } = new List<EarningsSentiment>();

        // Volatility and Risk
        public decimal MarketVolatility { get; set; }
        public decimal RiskLevel { get; set; }
        public string RiskCategory { get; set; } = string.Empty;
        public List<RiskFactor> RiskFactors { get; set; } = new List<RiskFactor>();

        // Forecast and Predictions
        public List<MarketPrediction> Predictions { get; set; } = new List<MarketPrediction>();
        public List<MarketTrendAnalysis> TrendAnalysis { get; set; } = new List<MarketTrendAnalysis>();

        // Display Properties
        public string DisplayOverallSentiment => OverallSentiment;
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplayBullishPercentage => $"{BullishPercentage:F1}%";
        public string DisplayBearishPercentage => $"{BearishPercentage:F1}%";
        public string DisplayNeutralPercentage => $"{NeutralPercentage:F1}%";
        public string DisplaySentimentChange => $"{SentimentChange:F1}";
        public string DisplayMarketVolatility => $"{MarketVolatility:F1}%";
        public string DisplayRiskLevel => $"{RiskLevel:F1}";

        // Calculation Methods
        public void CalculateMetrics()
        {
            if (MarketIndicators?.Any() == true)
            {
                TotalIndicators = MarketIndicators.Count;
                BullishIndicators = MarketIndicators.Count(i => i.Sentiment == SentimentType.Bullish);
                BearishIndicators = MarketIndicators.Count(i => i.Sentiment == SentimentType.Bearish);
                NeutralIndicators = MarketIndicators.Count(i => i.Sentiment == SentimentType.Neutral);

                if (TotalIndicators > 0)
                {
                    BullishPercentage = (decimal)BullishIndicators / TotalIndicators * 100;
                    BearishPercentage = (decimal)BearishIndicators / TotalIndicators * 100;
                    NeutralPercentage = (decimal)NeutralIndicators / TotalIndicators * 100;
                }
            }

            if (DailyData?.Any() == true)
            {
                var currentDay = DailyData.OrderByDescending(d => d.Date).FirstOrDefault();
                var previousDay = DailyData.OrderByDescending(d => d.Date).Skip(1).FirstOrDefault();

                if (currentDay != null)
                {
                    SentimentScore = currentDay.SentimentScore;
                    OverallSentiment = GetSentimentLabel(currentDay.SentimentScore);
                }

                if (currentDay != null && previousDay != null)
                {
                    SentimentChange = currentDay.SentimentScore - previousDay.SentimentScore;
                    PreviousSentimentScore = previousDay.SentimentScore;
                }
            }

            CalculateRiskMetrics();
            CalculateSectorMetrics();
        }

        private void CalculateRiskMetrics()
        {
            if (RiskFactors?.Any() == true)
            {
                RiskLevel = RiskFactors.Average(r => r.Severity);
                RiskCategory = GetRiskCategory(RiskLevel);
            }

            if (TechnicalIndicators?.Any() == true)
            {
                var volatilityIndicators = TechnicalIndicators.Where(t => t.Type == IndicatorType.Volatility);
                if (volatilityIndicators.Any())
                {
                    MarketVolatility = volatilityIndicators.Average(t => t.Value);
                }
            }
        }

        private void CalculateSectorMetrics()
        {
            if (SectorData?.Any() == true)
            {
                foreach (var sector in SectorData)
                {
                    sector.CalculateMetrics();
                }
            }
        }

        private string GetSentimentLabel(decimal score)
        {
            if (score >= 8.0m) return "Extremely Bullish";
            if (score >= 6.0m) return "Very Bullish";
            if (score >= 4.0m) return "Bullish";
            if (score >= 2.0m) return "Slightly Bullish";
            if (score >= -2.0m) return "Neutral";
            if (score >= -4.0m) return "Slightly Bearish";
            if (score >= -6.0m) return "Bearish";
            if (score >= -8.0m) return "Very Bearish";
            return "Extremely Bearish";
        }

        private string GetRiskCategory(decimal riskLevel)
        {
            if (riskLevel <= 2.0m) return "Low";
            if (riskLevel <= 4.0m) return "Moderate";
            if (riskLevel <= 6.0m) return "High";
            if (riskLevel <= 8.0m) return "Very High";
            return "Extreme";
        }

        public List<DailySentimentData> GetTopDays(int count = 5)
        {
            return DailyData?
                .OrderByDescending(d => d.SentimentScore)
                .Take(count)
                .ToList() ?? new List<DailySentimentData>();
        }

        public List<MarketIndicator> GetBullishIndicators()
        {
            return MarketIndicators?
                .Where(i => i.Sentiment == SentimentType.Bullish)
                .OrderByDescending(i => i.Strength)
                .ToList() ?? new List<MarketIndicator>();
        }

        public List<MarketIndicator> GetBearishIndicators()
        {
            return MarketIndicators?
                .Where(i => i.Sentiment == SentimentType.Bearish)
                .OrderByDescending(i => i.Strength)
                .ToList() ?? new List<MarketIndicator>();
        }

        public List<SectorSentiment> GetTopSectors(int count = 10)
        {
            return SectorData?
                .OrderByDescending(s => s.SentimentScore)
                .Take(count)
                .ToList() ?? new List<SectorSentiment>();
        }

        public List<IndustrySentiment> GetTopIndustries(int count = 10)
        {
            return IndustryData?
                .OrderByDescending(i => i.SentimentScore)
                .Take(count)
                .ToList() ?? new List<IndustrySentiment>();
        }

        public decimal GetWeeklySentimentChange()
        {
            if (WeeklyData?.Count < 2) return 0;

            var currentWeek = WeeklyData.OrderByDescending(w => w.WeekStart).First();
            var previousWeek = WeeklyData.OrderByDescending(w => w.WeekStart).Skip(1).First();

            if (previousWeek.AverageSentiment == 0) return 0;

            return ((currentWeek.AverageSentiment - previousWeek.AverageSentiment) / previousWeek.AverageSentiment) * 100;
        }

        public string GetSentimentTrend()
        {
            var weeklyChange = GetWeeklySentimentChange();
            
            if (weeklyChange > 15) return "Strongly Improving";
            if (weeklyChange > 8) return "Improving";
            if (weeklyChange > -8) return "Stable";
            if (weeklyChange > -15) return "Declining";
            return "Strongly Declining";
        }

        public List<RiskFactor> GetHighRiskFactors(decimal threshold = 6.0m)
        {
            return RiskFactors?
                .Where(r => r.Severity >= threshold)
                .OrderByDescending(r => r.Severity)
                .ToList() ?? new List<RiskFactor>();
        }

        public List<MarketPrediction> GetShortTermPredictions()
        {
            return Predictions?
                .Where(p => p.Timeframe == PredictionTimeframe.ShortTerm)
                .OrderBy(p => p.PredictionDate)
                .ToList() ?? new List<MarketPrediction>();
        }

        public List<MarketEvent> GetRecentEvents(int count = 10)
        {
            return RecentEvents?
                .OrderByDescending(e => e.EventDate)
                .Take(count)
                .ToList() ?? new List<MarketEvent>();
        }

        public decimal GetSectorSentiment(string sector)
        {
            var sectorData = SectorData?.FirstOrDefault(s => s.SectorName.Equals(sector, StringComparison.OrdinalIgnoreCase));
            return sectorData?.SentimentScore ?? 0;
        }

        public List<NewsSentiment> GetPositiveNews(int count = 10)
        {
            return NewsSentiment?
                .Where(n => n.SentimentScore > 0)
                .OrderByDescending(n => n.SentimentScore)
                .Take(count)
                .ToList() ?? new List<NewsSentiment>();
        }
    }

    // Supporting Models
    public class DailySentimentData
    {
        public DateTime Date { get; set; }
        public decimal SentimentScore { get; set; }
        public int TotalIndicators { get; set; }
        public int BullishIndicators { get; set; }
        public int BearishIndicators { get; set; }
        public int NeutralIndicators { get; set; }
        public decimal MarketVolatility { get; set; }
        public string DisplayDate => Date.ToString("MMM dd, yyyy");
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayMarketVolatility => $"{MarketVolatility:F1}%";
    }

    public class WeeklySentimentData
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public decimal AverageSentiment { get; set; }
        public decimal HighSentiment { get; set; }
        public decimal LowSentiment { get; set; }
        public decimal AverageVolatility { get; set; }
        public string DisplayWeek => $"{WeekStart:MMM dd} - {WeekEnd:MMM dd, yyyy}";
        public string DisplayAverageSentiment => $"{AverageSentiment:F1}";
        public string DisplayAverageVolatility => $"{AverageVolatility:F1}%";
    }

    public class MonthlySentimentData
    {
        public DateTime Month { get; set; }
        public decimal AverageSentiment { get; set; }
        public decimal HighSentiment { get; set; }
        public decimal LowSentiment { get; set; }
        public decimal AverageVolatility { get; set; }
        public int TotalDays { get; set; }
        public string DisplayMonth => Month.ToString("MMM yyyy");
        public string DisplayAverageSentiment => $"{AverageSentiment:F1}";
        public string DisplayAverageVolatility => $"{AverageVolatility:F1}%";
    }

    public class MarketIndicator
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public SentimentType Sentiment { get; set; }
        public decimal Strength { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DisplayStrength => $"{Strength:F1}";
        public string DisplayValue => $"{Value:F2}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
        public string DisplaySentiment => Sentiment.ToString();
    }

    public class SentimentIndicator
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public DateTime LastUpdated { get; set; }
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
    }

    public class TechnicalIndicator
    {
        public string Name { get; set; } = string.Empty;
        public IndicatorType Type { get; set; }
        public decimal Value { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal Change { get; set; }
        public string Signal { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public string DisplayValue => $"{Value:F2}";
        public string DisplayChange => $"{Change:F2}";
        public string DisplayLastUpdated => LastUpdated.ToString("MMM dd, yyyy");
    }

    public class SectorSentiment
    {
        public string SectorName { get; set; } = string.Empty;
        public string SectorCode { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public int TotalCompanies { get; set; }
        public int BullishCompanies { get; set; }
        public int BearishCompanies { get; set; }

        public void CalculateMetrics()
        {
            Change = SentimentScore - PreviousScore;
        }

        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
        public string DisplayBullishCompanies => $"{BullishCompanies:N0}";
        public string DisplayBearishCompanies => $"{BearishCompanies:N0}";
    }

    public class IndustrySentiment
    {
        public string IndustryName { get; set; } = string.Empty;
        public string NAICSCode { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public int TotalCompanies { get; set; }
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
        public string DisplayTotalCompanies => $"{TotalCompanies:N0}";
    }

    public class RegionalSentiment
    {
        public string Region { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public int TotalMarkets { get; set; }
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
        public string DisplayTotalMarkets => $"{TotalMarkets:N0}";
    }

    public class CountrySentiment
    {
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public int TotalMarkets { get; set; }
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
        public string DisplayTotalMarkets => $"{TotalMarkets:N0}";
    }

    public class MarketEvent
    {
        public string EventTitle { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public decimal SentimentImpact { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AffectedSectors { get; set; } = string.Empty;
        public string DisplayEventDate => EventDate.ToString("MMM dd, yyyy");
        public string DisplaySentimentImpact => $"{SentimentImpact:F1}";
    }

    public class NewsSentiment
    {
        public string Headline { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public decimal SentimentScore { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string DisplayPublicationDate => PublicationDate.ToString("MMM dd, yyyy");
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
    }

    public class EarningsSentiment
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public DateTime EarningsDate { get; set; }
        public decimal SentimentScore { get; set; }
        public decimal PreviousScore { get; set; }
        public decimal Change { get; set; }
        public string Sector { get; set; } = string.Empty;
        public string DisplayEarningsDate => EarningsDate.ToString("MMM dd, yyyy");
        public string DisplaySentimentScore => $"{SentimentScore:F1}";
        public string DisplayChange => $"{Change:F1}";
    }

    public class RiskFactor
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Severity { get; set; }
        public decimal Probability { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Mitigation { get; set; } = string.Empty;
        public string DisplaySeverity => $"{Severity:F1}";
        public string DisplayProbability => $"{Probability:F1}%";
    }

    public class MarketPrediction
    {
        public string PredictionTitle { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime PredictionDate { get; set; }
        public DateTime TargetDate { get; set; }
        public PredictionTimeframe Timeframe { get; set; }
        public decimal PredictedSentiment { get; set; }
        public string Rationale { get; set; } = string.Empty;
        public string DisplayPredictionDate => PredictionDate.ToString("MMM dd, yyyy");
        public string DisplayTargetDate => TargetDate.ToString("MMM dd, yyyy");
        public string DisplayPredictedSentiment => $"{PredictedSentiment:F1}";
    }

    public class MarketTrendAnalysis
    {
        public string TrendName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Strength { get; set; }
        public decimal Duration { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DisplayStrength => $"{Strength:F1}";
        public string DisplayDuration => $"{Duration:F1} days";
    }

    public enum SentimentType
    {
        Bullish,
        Bearish,
        Neutral
    }

    public enum IndicatorType
    {
        Momentum,
        Volatility,
        Trend,
        Volume,
        Price
    }

    public enum PredictionTimeframe
    {
        ShortTerm,
        MediumTerm,
        LongTerm
    }
}
