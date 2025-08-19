# Public Officials Models Documentation

This document describes the complete model structure for the "Check Out What Your Public Officials Are Up To" section in the Ezana application. The models are organized in subfolders exactly like the dashboard cards models, with each card having its own dedicated model file and embedded calculation logic.

## Overview

The Public Officials models provide comprehensive data structures for tracking and analyzing various aspects of public official activities, including congressional trading, government contracts, lobbying activity, patent momentum, and market sentiment. Each model includes embedded calculation logic for metrics, trends, and insights.

## Model Structure

### Directory Organization
```
Models/
└── PublicOfficials/
    ├── HistoricalCongressTradingCard.cs
    ├── GovernmentContractsCard.cs
    ├── HouseTradingCard.cs
    ├── LobbyingActivityCard.cs
    ├── SenatorTradingCard.cs
    ├── PatentMomentumCard.cs
    ├── MarketSentimentCard.cs
    └── PublicOfficialsSummary.cs
```

## Individual Card Models

### 1. HistoricalCongressTradingCard
**Purpose**: Tracks congressional trading activities and compliance metrics.

**Key Properties**:
- `TotalTrades`, `TotalVolume`, `ActiveTraders`
- `AverageTradeSize`, `LargestTrade`, `SmallestTrade`
- `MonthlyData`, `YearlyData` for time-based analysis
- `TopTraders`, `TopCompanies` for performance ranking
- `RecentTrades`, `NotableTrades` for activity tracking
- `PotentialConflicts`, `ConflictAlerts` for compliance monitoring

**Calculation Methods**:
- `CalculateMetrics()` - Computes all derived metrics
- `GetMonthlyGrowthRate()` - Calculates month-over-month growth
- `GetTradingTrend()` - Determines overall trading trend
- `GetHighPriorityConflicts()` - Identifies high-risk conflicts

**Supporting Models**:
- `MonthlyTradingData`, `YearlyTradingData`
- `TopCongressTrader`, `TopTradedCompany`
- `RecentTrade`, `NotableTrade`, `ConflictAlert`

### 2. GovernmentContractsCard
**Purpose**: Monitors government contract awards, spending, and performance.

**Key Properties**:
- `TotalContracts`, `TotalValue`, `ActiveCompanies`
- `AverageContractValue`, `LargestContract`, `SmallestContract`
- `MonthlyData`, `YearlyData`, `QuarterlyData`
- `TopContractors`, `TopAgencies`, `TopCategories`
- `RecentContracts`, `NotableContracts`
- `CompletionRate`, `OnTimeDeliveryRate`, `QualityRating`

**Calculation Methods**:
- `CalculateMetrics()` - Computes contract performance metrics
- `GetMonthlyGrowthRate()` - Tracks contract value trends
- `GetContractTrend()` - Determines overall contract trend
- `GetHighValueContracts()` - Identifies major contracts

**Supporting Models**:
- `MonthlyContractData`, `YearlyContractData`, `QuarterlyContractData`
- `TopContractor`, `TopContractingAgency`, `TopContractCategory`
- `RecentContract`, `NotableContract`
- `StateContractData`, `RegionContractData`

### 3. HouseTradingCard
**Purpose**: Tracks House of Representatives trading activities and performance.

**Key Properties**:
- `TotalTrades`, `TotalVolume`, `ActiveTraders`
- `AverageTradeSize`, `LargestTrade`, `SmallestTrade`
- `MonthlyData`, `YearlyData`, `QuarterlyData`
- `TopTraders`, `TopCompanies`, `TopSectors`
- `PartyData`, `CommitteeData`, `StateData`
- `AverageReturn`, `BestPerformingTrader`, `WorstPerformingTrader`

**Calculation Methods**:
- `CalculateMetrics()` - Computes trading and performance metrics
- `GetPartyRanking()` - Ranks parties by trading volume
- `GetTopCommittees()` - Identifies most active committees
- `GetHighValueTrades()` - Finds significant trading activity

**Supporting Models**:
- `MonthlyHouseTradingData`, `YearlyHouseTradingData`, `QuarterlyHouseTradingData`
- `TopHouseTrader`, `TopHouseTradedCompany`, `TopHouseSector`
- `RecentHouseTrade`, `NotableHouseTrade`
- `PartyTradingData`, `CommitteeTradingData`, `StateHouseTradingData`

### 4. LobbyingActivityCard
**Purpose**: Monitors lobbying activities, spending, and compliance.

**Key Properties**:
- `TotalLobbyingReports`, `TotalSpending`, `ActiveLobbyingFirms`
- `AverageSpending`, `HighestSpending`, `LowestSpending`
- `MonthlyData`, `YearlyData`, `QuarterlyData`
- `TopFirms`, `TopCompanies`, `TopIssues`
- `IssueData`, `IndustryData`, `CategoryData`
- `ComplianceRate`, `LateReports`, `IncompleteReports`

**Calculation Methods**:
- `CalculateMetrics()` - Computes lobbying and compliance metrics
- `GetLobbyingTrend()` - Determines spending trends
- `GetTopIssues()` - Identifies most lobbied issues
- `GetHighPriorityAlerts()` - Finds compliance issues

**Supporting Models**:
- `MonthlyLobbyingData`, `YearlyLobbyingData`, `QuarterlyLobbyingData`
- `TopLobbyingFirm`, `TopLobbyingCompany`, `TopLobbyingIssue`
- `RecentLobbyingReport`, `NotableLobbyingActivity`
- `IssueBreakdown`, `IndustryLobbyingData`, `LobbyingCategory`

### 5. SenatorTradingCard
**Purpose**: Tracks Senate trading activities and performance analysis.

**Key Properties**:
- `TotalTrades`, `TotalVolume`, `ActiveTraders`
- `AverageTradeSize`, `LargestTrade`, `SmallestTrade`
- `MonthlyData`, `YearlyData`, `QuarterlyData`
- `TopTraders`, `TopCompanies`, `TopSectors`
- `PartyData`, `CommitteeData`, `StateData`
- `TermData`, `ReelectionData` for political analysis

**Calculation Methods**:
- `CalculateMetrics()` - Computes trading and performance metrics
- `GetTermPerformance()` - Analyzes trading by term
- `GetReelectionTrading()` - Tracks trading around elections
- `GetHighValueTrades()` - Identifies significant trades

**Supporting Models**:
- `MonthlySenatorTradingData`, `YearlySenatorTradingData`, `QuarterlySenatorTradingData`
- `TopSenatorTrader`, `TopSenatorTradedCompany`, `TopSenatorSector`
- `RecentSenatorTrade`, `NotableSenatorTrade`
- `TermTradingData`, `ReelectionTradingData`

### 6. PatentMomentumCard
**Purpose**: Tracks patent activity, innovation metrics, and technology trends.

**Key Properties**:
- `TotalPatents`, `ActivePatents`, `PendingPatents`
- `AveragePatentsPerCompany`, `AveragePatentsPerInventor`
- `MonthlyData`, `YearlyData`, `QuarterlyData`
- `TopCompanies`, `TopInventors`, `TopTechnologies`
- `TechnologyData`, `IndustryData`, `CategoryData`
- `AveragePatentQuality`, `CitationRate`, `LitigationRate`

**Calculation Methods**:
- `CalculateMetrics()` - Computes patent and innovation metrics
- `GetPatentTrend()` - Determines innovation trends
- `GetHighQualityPatents()` - Identifies impactful patents
- `GetEmergingTechnologies()` - Finds growing technology areas

**Supporting Models**:
- `MonthlyPatentData`, `YearlyPatentData`, `QuarterlyPatentData`
- `TopPatentCompany`, `TopInventor`, `TopPatentTechnology`
- `RecentPatent`, `NotablePatent`
- `TechnologyBreakdown`, `IndustryPatentData`, `PatentCategory`

### 7. MarketSentimentCard
**Purpose**: Analyzes market sentiment, indicators, and risk factors.

**Key Properties**:
- `OverallSentiment`, `SentimentScore`, `TotalIndicators`
- `BullishIndicators`, `BearishIndicators`, `NeutralIndicators`
- `DailyData`, `WeeklyData`, `MonthlyData`
- `MarketIndicators`, `SentimentIndicators`, `TechnicalIndicators`
- `SectorData`, `IndustryData`, `RegionalData`
- `MarketVolatility`, `RiskLevel`, `RiskCategory`

**Calculation Methods**:
- `CalculateMetrics()` - Computes sentiment and risk metrics
- `GetSentimentTrend()` - Determines sentiment direction
- `GetHighRiskFactors()` - Identifies risk concerns
- `GetPositiveNews()` - Finds bullish news items

**Supporting Models**:
- `DailySentimentData`, `WeeklySentimentData`, `MonthlySentimentData`
- `MarketIndicator`, `SentimentIndicator`, `TechnicalIndicator`
- `SectorSentiment`, `IndustrySentiment`, `RegionalSentiment`
- `MarketEvent`, `NewsSentiment`, `EarningsSentiment`

## Summary Model

### PublicOfficialsSummary
**Purpose**: Comprehensive dashboard that combines all public officials cards with cross-card analysis.

**Key Properties**:
- All individual card models as properties
- `TotalDataPoints`, `OverallComplianceScore`, `OverallRiskLevel`
- `DataQualityScore`, `UpdateFrequency`, `CoverageScore`
- `Insights`, `Alerts`, `Correlations`, `RiskAssessments`

**Calculation Methods**:
- `CalculateSummaryMetrics()` - Computes all summary metrics
- `GenerateInsights()` - Creates cross-card insights
- `GenerateAlerts()` - Identifies system-wide alerts
- `AnalyzeCrossCardCorrelations()` - Finds data relationships

**Supporting Models**:
- `PublicOfficialInsight`, `PublicOfficialAlert`
- `CrossCardCorrelation`, `TrendAnalysis`, `RiskAssessment`

## Key Features

### 1. Embedded Calculation Logic
Each model contains its own calculation methods, eliminating the need for external calculation services and ensuring data consistency.

### 2. Display Properties
All models include computed display properties that format data appropriately for UI presentation (e.g., currency formatting, date formatting, percentages).

### 3. Time-based Analysis
Most models include monthly, quarterly, and yearly data structures for trend analysis and historical comparisons.

### 4. Risk and Compliance Monitoring
Trading and lobbying models include conflict detection and compliance scoring for regulatory oversight.

### 5. Cross-Card Analysis
The summary model provides insights and correlations between different data sources for comprehensive analysis.

### 6. Performance Metrics
Models include performance tracking, quality metrics, and health status indicators for data reliability assessment.

## Usage Examples

### Basic Model Usage
```csharp
var congressCard = new HistoricalCongressTradingCard();
congressCard.CalculateMetrics();
var trend = congressCard.GetTradingTrend();
var topTraders = congressCard.GetTopTradersByVolume(10);
```

### Summary Dashboard Usage
```csharp
var summary = new PublicOfficialsSummary();
summary.CongressTrading = congressData;
summary.GovernmentContracts = contractData;
// ... populate other cards
summary.CalculateSummaryMetrics();

var insights = summary.GetHighPriorityInsights();
var alerts = summary.GetHighPriorityAlerts();
```

### Data Quality Assessment
```csharp
var dataHealth = summary.DataHealthStatus;
var qualityScore = summary.DataQualityScore;
var updateFrequency = summary.UpdateFrequency;
```

## Data Flow

1. **Data Ingestion**: External data sources populate individual card models
2. **Calculation**: Each model calculates its own metrics and derived values
3. **Aggregation**: Summary model combines all cards and performs cross-analysis
4. **Insight Generation**: Automated insights and alerts are generated based on data patterns
5. **Presentation**: Display properties provide formatted data for UI consumption

## Future Enhancements

### Planned Features
- Real-time data streaming integration
- Advanced machine learning for trend prediction
- Enhanced compliance monitoring algorithms
- Integration with additional government data sources
- Advanced correlation analysis and pattern recognition

### Extensibility
The model structure is designed to easily accommodate:
- New data sources and card types
- Additional calculation methods
- Enhanced risk assessment algorithms
- Custom compliance rules and thresholds
- Integration with external analytics platforms

## Conclusion

The Public Officials models provide a comprehensive, self-contained system for tracking and analyzing public official activities. With embedded calculation logic, extensive supporting models, and cross-card analysis capabilities, these models serve as the foundation for a robust public accountability and market intelligence platform.

Each model is designed to be independently functional while contributing to the overall dashboard ecosystem, ensuring maintainability, scalability, and data integrity across the entire system.
