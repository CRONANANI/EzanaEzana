# Dashboard Card Models Documentation

This document outlines the comprehensive model structure for all dashboard cards in the Ezana application.

## Overview

We've created dedicated model files for each dashboard card type, providing clear data structures and display logic for the portfolio analytics dashboard. Each model includes:

- **Core Data Properties** - Raw data values
- **Display Properties** - Formatted values for UI consumption
- **Validation Attributes** - Data annotations for validation
- **Helper Methods** - Business logic for calculations and formatting
- **Enums** - Categorized values and statuses

## Model Structure

### 1. PortfolioValueCard (`Models/DashboardCards/PortfolioValueCard.cs`)

**Purpose**: Total portfolio value and performance metrics

**Key Properties**:
- `TotalValue` - Current portfolio value
- `TotalReturn` - Absolute return amount
- `ReturnPercentage` - Percentage return
- `TodayPnl` - Today's profit/loss
- `TodayPnlPercentage` - Today's P&L percentage
- `TopHoldings` - Top portfolio holdings
- `RecentTrades` - Recent trading activity
- `PerformanceHistory` - Historical performance data

**Display Features**:
- Formatted currency values
- Formatted percentages
- Formatted dates
- Positive/negative indicators for styling

### 2. TodaysPnlCard (`Models/DashboardCards/TodaysPnlCard.cs`)

**Purpose**: Today's profit/loss tracking and market session status

**Key Properties**:
- `TodayPnl` - Today's P&L amount
- `TodayPnlPercentage` - Today's P&L percentage
- `MarketStatus` - Current market session status
- `PreviousDayPnl` - Previous day's P&L for comparison
- `WeekToDatePnl` - Week-to-date P&L
- `MonthToDatePnl` - Month-to-date P&L

**Display Features**:
- Market session indicators
- P&L change comparisons
- Formatted time periods
- Trend indicators

### 3. TopPerformerCard (`Models/DashboardCards/TopPerformerCard.cs`)

**Purpose**: Best performing asset in the portfolio

**Key Properties**:
- `Ticker` - Stock symbol
- `Company` - Company name
- `ReturnAmount` - Return in dollars
- `ReturnPercentage` - Return percentage
- `Shares` - Number of shares owned
- `CurrentValue` - Current holding value
- `Sector` - Industry classification

**Display Features**:
- Performance metrics
- Sector information
- Share count formatting
- Price change indicators

### 4. RiskScoreCard (`Models/DashboardCards/RiskScoreCard.cs`)

**Purpose**: Portfolio risk assessment and analysis

**Key Properties**:
- `Score` - Risk score (0-10 scale)
- `RiskCategory` - Risk classification
- `PreviousScore` - Previous risk score
- `ScoreChange` - Change in risk score
- `RiskFactors` - Individual risk components
- `Volatility` - Portfolio volatility measure
- `Beta` - Market correlation
- `SharpeRatio` - Risk-adjusted return

**Display Features**:
- Risk level indicators
- Color-coded risk levels
- Risk factor breakdowns
- Performance metrics

### 5. MonthlyDividendsCard (`Models/DashboardCards/MonthlyDividendsCard.cs`)

**Purpose**: Monthly dividend income tracking

**Key Properties**:
- `MonthlyAmount` - Current month's dividends
- `PreviousMonthAmount` - Previous month's dividends
- `ChangeAmount` - Month-over-month change
- `ChangePercentage` - Percentage change
- `NextPaymentDate` - Next dividend payment
- `UpcomingPayments` - Future dividend payments
- `RecentPayments` - Past dividend payments

**Display Features**:
- Dividend change indicators
- Payment date formatting
- Payment status tracking
- Income projections

### 6. AssetAllocationCard (`Models/DashboardCards/AssetAllocationCard.cs`)

**Purpose**: Portfolio diversification and allocation analysis

**Key Properties**:
- `BreakdownType` - Allocation breakdown method
- `Items` - Individual allocation items
- `DiversificationScore` - Diversification rating (0-100)
- `RiskAssessment` - Risk evaluation
- `RebalancingRecommendations` - Rebalancing suggestions
- `TargetAllocations` - Target percentages
- `AllocationDeviations` - Current vs. target deviations

**Display Features**:
- Diversification scoring
- Rebalancing alerts
- Target vs. actual comparisons
- Risk level indicators

### 7. DashboardCardsSummary (`Models/DashboardCards/DashboardCardsSummary.cs`)

**Purpose**: Comprehensive dashboard combining all card data

**Key Properties**:
- All individual card models
- `PortfolioHealthScore` - Overall portfolio health (0-100)
- `HealthStatus` - Portfolio health classification
- `Insights` - Key recommendations
- `Alerts` - Important notifications

**Display Features**:
- Portfolio health scoring
- Alert summaries
- Insight collections
- Overall status indicators

## Data Validation

All models include comprehensive validation using `System.ComponentModel.DataAnnotations`:

- **Required Fields** - Essential data properties
- **Range Validation** - Numeric value constraints
- **Display Names** - User-friendly property labels
- **String Length** - Text field limitations

## Display Logic

Each model includes a `DisplayValues` property that provides:

- **Formatted Values** - Currency, percentage, and date formatting
- **UI Indicators** - Boolean flags for styling (positive/negative, etc.)
- **Color Codes** - Hex colors for risk levels and status indicators
- **Calculated Fields** - Derived values for display

## Business Logic

Models include helper methods for:

- **Score Calculations** - Risk and health scoring algorithms
- **Status Determinations** - Risk level and health status logic
- **Color Assignments** - Dynamic color coding based on values
- **Formatting** - Consistent display formatting across the application

## Usage Examples

### Creating a Portfolio Value Card

```csharp
var portfolioCard = new PortfolioValueCard
{
    TotalValue = 125000.00m,
    TotalReturn = 15000.00m,
    ReturnPercentage = 13.6m,
    TodayPnl = 1250.00m,
    TodayPnlPercentage = 1.0m,
    LastUpdated = DateTime.Now
};

// Access formatted display values
var displayValue = portfolioCard.DisplayValues.TotalValueFormatted; // "$125,000.00"
var isPositive = portfolioCard.DisplayValues.IsPositiveReturn; // true
```

### Working with Risk Score

```csharp
var riskCard = new RiskScoreCard
{
    Score = 4.2m,
    PreviousScore = 4.8m,
    ScoreChange = -0.6m,
    RiskCategory = RiskCategory.Moderate
};

// Get risk level and color
var riskLevel = riskCard.DisplayValues.RiskLevel; // RiskLevel.Moderate
var riskColor = riskCard.DisplayValues.RiskColor; // "#F59E0B" (Yellow)
```

### Asset Allocation Analysis

```csharp
var allocationCard = new AssetAllocationCard
{
    DiversificationScore = 75.0m,
    BreakdownType = "asset_class"
};

// Check if rebalancing is needed
var needsRebalancing = allocationCard.DisplayValues.NeedsRebalancing;
var largestDeviation = allocationCard.DisplayValues.LargestDeviationFormatted;
```

## Integration with Controllers

These models integrate seamlessly with the existing `DashboardCardsController`:

- **API Endpoints** - Return strongly-typed model data
- **View Models** - Provide structured data for Razor views
- **JSON Serialization** - Clean API responses
- **Validation** - Automatic model validation

## Future Enhancements

Potential improvements for these models:

1. **Real-time Updates** - SignalR integration for live data
2. **Caching** - Redis caching for performance
3. **Audit Trail** - Track changes to portfolio data
4. **Notifications** - Alert system integration
5. **Export** - Data export functionality
6. **Historical Tracking** - Time-series data analysis

## File Organization

```
Models/
└── DashboardCards/
    ├── PortfolioValueCard.cs
    ├── TodaysPnlCard.cs
    ├── TopPerformerCard.cs
    ├── RiskScoreCard.cs
    ├── MonthlyDividendsCard.cs
    ├── AssetAllocationCard.cs
    └── DashboardCardsSummary.cs
```

This structure provides a clean, maintainable, and extensible foundation for the dashboard card system.
