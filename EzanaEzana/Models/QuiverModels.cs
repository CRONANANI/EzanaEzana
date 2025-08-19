using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using EzanaEzana.Models;

namespace EzanaEzana.Models
{
    // Base models for Quiver API responses
    public class QuiverApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    // Congressional Trading Models
    public class CongressionalTrading
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Congressperson")]
        public string Congressperson { get; set; }

        [JsonPropertyName("Party")]
        public string Party { get; set; }

        [JsonPropertyName("Chamber")]
        public string Chamber { get; set; }

        [JsonPropertyName("Trade_Type")]
        public string TradeType { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("Owner")]
        public string Owner { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }
    }

    // Government Contracts Models
    public class GovernmentContract
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Contract_Value")]
        public decimal ContractValue { get; set; }

        [JsonPropertyName("Agency")]
        public string Agency { get; set; }

        [JsonPropertyName("Contract_Type")]
        public string ContractType { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }

    // House Trading Models
    public class HouseTrading
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Representative")]
        public string Representative { get; set; }

        [JsonPropertyName("Party")]
        public string Party { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Trade_Type")]
        public string TradeType { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }
    }

    // Senator Trading Models
    public class SenatorTrading
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Senator")]
        public string Senator { get; set; }

        [JsonPropertyName("Party")]
        public string Party { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Trade_Type")]
        public string TradeType { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }
    }

    // Lobbying Activity Models
    public class LobbyingActivity
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Lobbying_Firm")]
        public string LobbyingFirm { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("Issues")]
        public string Issues { get; set; }

        [JsonPropertyName("Registrant")]
        public string Registrant { get; set; }
    }

    // Patent Momentum Models
    public class PatentMomentum
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Patent_Count")]
        public int PatentCount { get; set; }

        [JsonPropertyName("Patent_Type")]
        public string PatentType { get; set; }

        [JsonPropertyName("Innovation_Score")]
        public decimal InnovationScore { get; set; }

        [JsonPropertyName("Industry")]
        public string Industry { get; set; }
    }

    // Congressperson Portfolio Models
    public class CongresspersonPortfolio
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Party")]
        public string Party { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Chamber")]
        public string Chamber { get; set; }

        [JsonPropertyName("Total_Portfolio_Value")]
        public decimal TotalPortfolioValue { get; set; }

        [JsonPropertyName("YTD_Return")]
        public decimal YtdReturn { get; set; }

        [JsonPropertyName("YTD_Return_Amount")]
        public decimal YtdReturnAmount { get; set; }

        [JsonPropertyName("Top_Holdings")]
        public List<PortfolioHolding> TopHoldings { get; set; } = new();

        [JsonPropertyName("Recent_Trades")]
        public List<PortfolioTrade> RecentTrades { get; set; } = new();
    }

    public class PortfolioTrade
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("Ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("Shares")]
        public int Shares { get; set; }
    }

    // Market Intelligence Data Models for Frontend
    public class MarketIntelligenceData
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public List<string> Columns { get; set; }
        public List<object> Data { get; set; }
        public int TotalCount { get; set; }
    }

    // Dashboard Models
    public class PortfolioSummary
    {
        public decimal TotalPortfolioValue { get; set; }
        public decimal TotalReturn { get; set; }
        public decimal ReturnPercentage { get; set; }
        public decimal TodayPnl { get; set; }
        public decimal TodayPnlPercentage { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<PortfolioHolding> TopHoldings { get; set; } = new();
        public List<PortfolioTrade> RecentTrades { get; set; } = new();
    }

    public class RiskScore
    {
        public decimal Score { get; set; } // 0-10 scale
        public string RiskCategory { get; set; } // Low, Moderate, High
        public decimal PreviousScore { get; set; }
        public decimal ScoreChange { get; set; }
        public string Description { get; set; }
        public List<RiskFactor> RiskFactors { get; set; } = new();
        public DateTime LastCalculated { get; set; }
    }

    public class RiskFactor
    {
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
    }

    public class DividendIncome
    {
        public decimal MonthlyAmount { get; set; }
        public decimal PreviousMonthAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public decimal ChangePercentage { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public List<DividendPayment> UpcomingPayments { get; set; } = new();
        public List<DividendPayment> RecentPayments { get; set; } = new();
    }

    public class DividendPayment
    {
        public string Ticker { get; set; }
        public string Company { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } // Upcoming, Paid, Declared
    }

    public class TopPerformer
    {
        public string Ticker { get; set; }
        public string Company { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal ReturnPercentage { get; set; }
        public int Shares { get; set; }
        public decimal CurrentValue { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class AssetAllocation
    {
        public string BreakdownType { get; set; } // "asset_class", "sector", "performance"
        public List<AssetAllocationItem> Items { get; set; } = new();
    }

    public class AssetAllocationItem
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public string Color { get; set; }
    }
}
