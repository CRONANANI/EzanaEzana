using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EzanaEzana.Models.PublicOfficials;

namespace EzanaEzana.Models.PublicOfficials
{
    public class PublicOfficialsSummary
    {
        // Core Dashboard Cards
        public HistoricalCongressTradingCard CongressTrading { get; set; } = new HistoricalCongressTradingCard();
        public GovernmentContractsCard GovernmentContracts { get; set; } = new GovernmentContractsCard();
        public HouseTradingCard HouseTrading { get; set; } = new HouseTradingCard();
        public LobbyingActivityCard LobbyingActivity { get; set; } = new LobbyingActivityCard();
        public SenatorTradingCard SenatorTrading { get; set; } = new SenatorTradingCard();
        public PatentMomentumCard PatentMomentum { get; set; } = new PatentMomentumCard();
        public MarketSentimentCard MarketSentiment { get; set; } = new MarketSentimentCard();

        // Summary Metrics
        public DateTime LastRefreshed { get; set; }
        public int TotalDataPoints { get; set; }
        public decimal OverallComplianceScore { get; set; }
        public string OverallRiskLevel { get; set; } = string.Empty;
        public List<PublicOfficialInsight> Insights { get; set; } = new List<PublicOfficialInsight>();
        public List<PublicOfficialAlert> Alerts { get; set; } = new List<PublicOfficialAlert>();

        // Cross-Card Analysis
        public List<CrossCardCorrelation> Correlations { get; set; } = new List<CrossCardCorrelation>();
        public List<PublicOfficialsTrendAnalysis> CrossCardTrends { get; set; } = new List<PublicOfficialsTrendAnalysis>();
        public List<RiskAssessment> RiskAssessments { get; set; } = new List<RiskAssessment>();

        // Performance Metrics
        public decimal DataQualityScore { get; set; }
        public decimal UpdateFrequency { get; set; }
        public decimal CoverageScore { get; set; }
        public string DataHealthStatus { get; set; } = string.Empty;

        // Display Properties
        public string DisplayLastRefreshed => LastRefreshed.ToString("MMM dd, yyyy 'at' HH:mm");
        public string DisplayTotalDataPoints => $"{TotalDataPoints:N0}";
        public string DisplayOverallComplianceScore => $"{OverallComplianceScore:F1}%";
        public string DisplayDataQualityScore => $"{DataQualityScore:F1}%";
        public string DisplayUpdateFrequency => $"{UpdateFrequency:F1}%";
        public string DisplayCoverageScore => $"{CoverageScore:F1}%";

        // Calculation Methods
        public void CalculateSummaryMetrics()
        {
            CalculateTotalDataPoints();
            CalculateOverallComplianceScore();
            CalculateOverallRiskLevel();
            CalculateDataQualityMetrics();
            GenerateInsights();
            GenerateAlerts();
            AnalyzeCrossCardCorrelations();
        }

        private void CalculateTotalDataPoints()
        {
            TotalDataPoints = 0;

            // Congress Trading
            if (CongressTrading?.RecentTrades != null)
                TotalDataPoints += CongressTrading.RecentTrades.Count;

            // Government Contracts
            if (GovernmentContracts?.RecentContracts != null)
                TotalDataPoints += GovernmentContracts.RecentContracts.Count;

            // House Trading
            if (HouseTrading?.RecentTrades != null)
                TotalDataPoints += HouseTrading.RecentTrades.Count;

            // Lobbying Activity
            if (LobbyingActivity?.RecentReports != null)
                TotalDataPoints += LobbyingActivity.RecentReports.Count;

            // Senator Trading
            if (SenatorTrading?.RecentTrades != null)
                TotalDataPoints += SenatorTrading.RecentTrades.Count;

            // Patent Momentum
            if (PatentMomentum?.RecentPatents != null)
                TotalDataPoints += PatentMomentum.RecentPatents.Count;

            // Market Sentiment
            if (MarketSentiment?.MarketIndicators != null)
                TotalDataPoints += MarketSentiment.MarketIndicators.Count;
        }

        private void CalculateOverallComplianceScore()
        {
            var scores = new List<decimal>();

            if (CongressTrading?.ComplianceScore > 0)
                scores.Add(CongressTrading.ComplianceScore);

            if (GovernmentContracts?.ComplianceRate > 0)
                scores.Add(GovernmentContracts.ComplianceRate);

            if (LobbyingActivity?.ComplianceRate > 0)
                scores.Add(LobbyingActivity.ComplianceRate);

            if (scores.Any())
            {
                OverallComplianceScore = scores.Average();
            }
        }

        private void CalculateOverallRiskLevel()
        {
            var riskFactors = new List<decimal>();

            // Congress Trading conflicts
            if (CongressTrading?.PotentialConflicts > 0)
                riskFactors.Add(CongressTrading.PotentialConflicts * 0.1m);

            // House Trading conflicts
            if (HouseTrading?.PotentialConflicts > 0)
                riskFactors.Add(HouseTrading.PotentialConflicts * 0.1m);

            // Senator Trading conflicts
            if (SenatorTrading?.PotentialConflicts > 0)
                riskFactors.Add(SenatorTrading.PotentialConflicts * 0.1m);

            // Market volatility
            if (MarketSentiment?.MarketVolatility > 0)
                riskFactors.Add(MarketSentiment.MarketVolatility * 0.05m);

            if (riskFactors.Any())
            {
                var averageRisk = riskFactors.Average();
                OverallRiskLevel = GetRiskLevelLabel(averageRisk);
            }
        }

        private string GetRiskLevelLabel(decimal riskScore)
        {
            if (riskScore <= 2.0m) return "Low";
            if (riskScore <= 4.0m) return "Moderate";
            if (riskScore <= 6.0m) return "High";
            if (riskScore <= 8.0m) return "Very High";
            return "Extreme";
        }

        private void CalculateDataQualityMetrics()
        {
            var qualityScores = new List<decimal>();
            var updateScores = new List<decimal>();
            var coverageScores = new List<decimal>();

            // Calculate data quality based on completeness and consistency
            if (CongressTrading?.RecentTrades?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(CongressTrading.RecentTrades.Count, CongressTrading.TotalTrades));
                updateScores.Add(CalculateUpdateFrequency(CongressTrading.LastUpdated));
            }

            if (GovernmentContracts?.RecentContracts?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(GovernmentContracts.RecentContracts.Count, GovernmentContracts.TotalContracts));
                updateScores.Add(CalculateUpdateFrequency(GovernmentContracts.LastUpdated));
            }

            if (HouseTrading?.RecentTrades?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(HouseTrading.RecentTrades.Count, HouseTrading.TotalTrades));
                updateScores.Add(CalculateUpdateFrequency(HouseTrading.LastUpdated));
            }

            if (LobbyingActivity?.RecentReports?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(LobbyingActivity.RecentReports.Count, LobbyingActivity.TotalLobbyingReports));
                updateScores.Add(CalculateUpdateFrequency(LobbyingActivity.LastUpdated));
            }

            if (SenatorTrading?.RecentTrades?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(SenatorTrading.RecentTrades.Count, SenatorTrading.TotalTrades));
                updateScores.Add(CalculateUpdateFrequency(SenatorTrading.LastUpdated));
            }

            if (PatentMomentum?.RecentPatents?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(PatentMomentum.RecentPatents.Count, PatentMomentum.TotalPatents));
                updateScores.Add(CalculateUpdateFrequency(PatentMomentum.LastUpdated));
            }

            if (MarketSentiment?.MarketIndicators?.Any() == true)
            {
                qualityScores.Add(CalculateDataQuality(MarketSentiment.MarketIndicators.Count, MarketSentiment.TotalIndicators));
                updateScores.Add(CalculateUpdateFrequency(MarketSentiment.LastUpdated));
            }

            // Calculate coverage based on available data vs expected data
            coverageScores.Add(CalculateCoverageScore());

            if (qualityScores.Any())
                DataQualityScore = qualityScores.Average();

            if (updateScores.Any())
                UpdateFrequency = updateScores.Average();

            if (coverageScores.Any())
                CoverageScore = coverageScores.Average();

            DataHealthStatus = GetDataHealthStatus();
        }

        private decimal CalculateDataQuality(int recentCount, int totalCount)
        {
            if (totalCount == 0) return 0;
            return (decimal)recentCount / totalCount * 100;
        }

        private decimal CalculateUpdateFrequency(DateTime lastUpdated)
        {
            var daysSinceUpdate = (DateTime.Now - lastUpdated).TotalDays;
            if (daysSinceUpdate <= 1) return 100;
            if (daysSinceUpdate <= 7) return 80;
            if (daysSinceUpdate <= 30) return 60;
            if (daysSinceUpdate <= 90) return 40;
            return 20;
        }

        private decimal CalculateCoverageScore()
        {
            var availableCards = 0;
            var totalExpectedCards = 7; // Total number of cards

            if (CongressTrading?.TotalTrades > 0) availableCards++;
            if (GovernmentContracts?.TotalContracts > 0) availableCards++;
            if (HouseTrading?.TotalTrades > 0) availableCards++;
            if (LobbyingActivity?.TotalLobbyingReports > 0) availableCards++;
            if (SenatorTrading?.TotalTrades > 0) availableCards++;
            if (PatentMomentum?.TotalPatents > 0) availableCards++;
            if (MarketSentiment?.TotalIndicators > 0) availableCards++;

            return (decimal)availableCards / totalExpectedCards * 100;
        }

        private string GetDataHealthStatus()
        {
            var averageScore = (DataQualityScore + UpdateFrequency + CoverageScore) / 3;

            if (averageScore >= 90) return "Excellent";
            if (averageScore >= 80) return "Good";
            if (averageScore >= 70) return "Fair";
            if (averageScore >= 60) return "Poor";
            return "Critical";
        }

        private void GenerateInsights()
        {
            Insights.Clear();

            // Trading volume insights
            if (CongressTrading?.TotalVolume > 0 && HouseTrading?.TotalVolume > 0)
            {
                var congressVolume = CongressTrading.TotalVolume;
                var houseVolume = HouseTrading.TotalVolume;
                var totalVolume = congressVolume + houseVolume;

                if (congressVolume > houseVolume * 1.5m)
                {
                    Insights.Add(new PublicOfficialInsight
                    {
                        Category = "Trading Analysis",
                        Title = "Congress Dominates Trading Volume",
                        Description = $"Congressional trading volume (${congressVolume:C0}) significantly exceeds House trading volume (${houseVolume:C0})",
                        Priority = InsightPriority.Medium,
                        GeneratedDate = DateTime.Now
                    });
                }
            }

            // Compliance insights
            if (OverallComplianceScore < 80)
            {
                Insights.Add(new PublicOfficialInsight
                {
                    Category = "Compliance",
                    Title = "Low Overall Compliance",
                    Description = $"Overall compliance score is {OverallComplianceScore:F1}%, indicating potential regulatory concerns",
                    Priority = InsightPriority.High,
                    GeneratedDate = DateTime.Now
                });
            }

            // Patent momentum insights
            if (PatentMomentum?.GetMonthlyGrowthRate() > 20)
            {
                Insights.Add(new PublicOfficialInsight
                {
                    Category = "Innovation",
                    Title = "Strong Patent Growth",
                    Description = $"Patent momentum shows {PatentMomentum.GetMonthlyGrowthRate():F1}% monthly growth, indicating strong innovation activity",
                    Priority = InsightPriority.Low,
                    GeneratedDate = DateTime.Now
                });
            }

            // Market sentiment insights
            if (MarketSentiment?.SentimentScore > 6)
            {
                Insights.Add(new PublicOfficialInsight
                {
                    Category = "Market Sentiment",
                    Title = "Bullish Market Sentiment",
                    Description = $"Market sentiment is {MarketSentiment.OverallSentiment} with a score of {MarketSentiment.SentimentScore:F1}",
                    Priority = InsightPriority.Low,
                    GeneratedDate = DateTime.Now
                });
            }
        }

        private void GenerateAlerts()
        {
            Alerts.Clear();

            // High conflict alerts
            var highConflicts = new List<string>();
            if (CongressTrading?.PotentialConflicts > 10) highConflicts.Add($"Congress: {CongressTrading.PotentialConflicts}");
            if (HouseTrading?.PotentialConflicts > 10) highConflicts.Add($"House: {HouseTrading.PotentialConflicts}");
            if (SenatorTrading?.PotentialConflicts > 10) highConflicts.Add($"Senate: {SenatorTrading.PotentialConflicts}");

            if (highConflicts.Any())
            {
                Alerts.Add(new PublicOfficialAlert
                {
                    Category = "Conflict Alert",
                    Title = "High Conflict Levels Detected",
                    Description = $"Multiple high conflict levels detected: {string.Join(", ", highConflicts)}",
                    Priority = AlertPriority.High,
                    AlertDate = DateTime.Now
                });
            }

            // Data quality alerts
            if (DataQualityScore < 70)
            {
                Alerts.Add(new PublicOfficialAlert
                {
                    Category = "Data Quality",
                    Title = "Low Data Quality",
                    Description = $"Data quality score is {DataQualityScore:F1}%, indicating potential data integrity issues",
                    Priority = AlertPriority.Medium,
                    AlertDate = DateTime.Now
                });
            }

            // Update frequency alerts
            if (UpdateFrequency < 60)
            {
                Alerts.Add(new PublicOfficialAlert
                {
                    Category = "Data Freshness",
                    Title = "Data Update Delays",
                    Description = $"Update frequency score is {UpdateFrequency:F1}%, indicating potential data staleness",
                    Priority = AlertPriority.Medium,
                    AlertDate = DateTime.Now
                });
            }
        }

        private void AnalyzeCrossCardCorrelations()
        {
            Correlations.Clear();

            // Analyze correlations between different data sources
            if (CongressTrading?.TotalVolume > 0 && GovernmentContracts?.TotalValue > 0)
            {
                var correlation = CalculateCorrelation(
                    new[] { CongressTrading.TotalVolume, HouseTrading?.TotalVolume ?? 0, SenatorTrading?.TotalVolume ?? 0 },
                    new[] { GovernmentContracts.TotalValue, GovernmentContracts.TotalValue, GovernmentContracts.TotalValue }
                );

                Correlations.Add(new CrossCardCorrelation
                {
                    Source1 = "Trading Volume",
                    Source2 = "Government Contracts",
                    CorrelationCoefficient = correlation,
                    Strength = GetCorrelationStrength(correlation),
                    Description = "Correlation between trading volume and government contract values"
                });
            }
        }

        private decimal CalculateCorrelation(decimal[] values1, decimal[] values2)
        {
            if (values1.Length != values2.Length || values1.Length < 2) return 0;

            var mean1 = values1.Average();
            var mean2 = values2.Average();

            var numerator = 0.0m;
            var denominator1 = 0.0m;
            var denominator2 = 0.0m;

            for (int i = 0; i < values1.Length; i++)
            {
                var diff1 = values1[i] - mean1;
                var diff2 = values2[i] - mean2;

                numerator += diff1 * diff2;
                denominator1 += diff1 * diff1;
                denominator2 += diff2 * diff2;
            }

            if (denominator1 == 0 || denominator2 == 0) return 0;

            return numerator / (decimal)Math.Sqrt((double)(denominator1 * denominator2));
        }

        private string GetCorrelationStrength(decimal correlation)
        {
            var absCorrelation = Math.Abs((double)correlation);
            if (absCorrelation >= 0.8) return "Very Strong";
            if (absCorrelation >= 0.6) return "Strong";
            if (absCorrelation >= 0.4) return "Moderate";
            if (absCorrelation >= 0.2) return "Weak";
            return "Very Weak";
        }

        public List<PublicOfficialInsight> GetHighPriorityInsights()
        {
            return Insights?
                .Where(i => i.Priority == InsightPriority.High)
                .OrderByDescending(i => i.GeneratedDate)
                .ToList() ?? new List<PublicOfficialInsight>();
        }

        public List<PublicOfficialAlert> GetHighPriorityAlerts()
        {
            return Alerts?
                .Where(a => a.Priority == AlertPriority.High)
                .OrderByDescending(a => a.AlertDate)
                .ToList() ?? new List<PublicOfficialAlert>();
        }

        public decimal GetCardPerformance(string cardName)
        {
            return cardName.ToLower() switch
            {
                "congress trading" => CongressTrading?.ComplianceScore ?? 0,
                "government contracts" => GovernmentContracts?.ComplianceRate ?? 0,
                "house trading" => HouseTrading?.AverageReturn ?? 0,
                "lobbying activity" => LobbyingActivity?.ComplianceRate ?? 0,
                "senator trading" => SenatorTrading?.AverageReturn ?? 0,
                "patent momentum" => PatentMomentum?.AveragePatentQuality ?? 0,
                "market sentiment" => MarketSentiment?.SentimentScore ?? 0,
                _ => 0
            };
        }
    }

    // Supporting Models
    public class PublicOfficialInsight
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public InsightPriority Priority { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string DisplayGeneratedDate => GeneratedDate.ToString("MMM dd, yyyy");
    }

    public class PublicOfficialAlert
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AlertPriority Priority { get; set; }
        public DateTime AlertDate { get; set; }
        public string DisplayAlertDate => AlertDate.ToString("MMM dd, yyyy");
    }

    public class CrossCardCorrelation
    {
        public string Source1 { get; set; } = string.Empty;
        public string Source2 { get; set; } = string.Empty;
        public decimal CorrelationCoefficient { get; set; }
        public string Strength { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DisplayCorrelation => $"{CorrelationCoefficient:F3}";
    }

    public class PublicOfficialsTrendAnalysis
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

    public class RiskAssessment
    {
        public string RiskCategory { get; set; } = string.Empty;
        public decimal RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Mitigation { get; set; } = string.Empty;
        public string DisplayRiskScore => $"{RiskScore:F1}";
    }

    public enum InsightPriority
    {
        Low,
        Medium,
        High
    }

    public enum AlertPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
