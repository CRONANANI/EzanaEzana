using EzanaEzana.Models;

namespace EzanaEzana.Services
{
    public interface IQuiverService
    {
        // Congressional Trading
        Task<List<CongressionalTrading>> GetCongressionalTradingAsync(int limit = 100);
        Task<CongresspersonPortfolio> GetCongresspersonPortfolioAsync(string congresspersonName);
        
        // Government Contracts
        Task<List<GovernmentContract>> GetGovernmentContractsAsync(int limit = 100);
        
        // House Trading
        Task<List<HouseTrading>> GetHouseTradingAsync(int limit = 100);
        
        // Senator Trading
        Task<List<SenatorTrading>> GetSenatorTradingAsync(int limit = 100);
        
        // Lobbying Activity
        Task<List<LobbyingActivity>> GetLobbyingActivityAsync(int limit = 100);
        
        // Patent Momentum
        Task<List<PatentMomentum>> GetPatentMomentumAsync(int limit = 100);
        
        // Market Intelligence Summary
        Task<Dictionary<string, MarketIntelligenceData>> GetMarketIntelligenceSummaryAsync();
        
        // Portfolio Analytics
        Task<PortfolioSummary> GetPortfolioSummaryAsync(string userId);
        Task<AssetAllocation> GetAssetAllocationAsync(string userId, string breakdownType = "asset_class");
        Task<List<PortfolioHolding>> GetTopHoldingsAsync(string userId, int count = 3);
        
        // Dashboard Cards
        Task<RiskScore> GetRiskScoreAsync(string userId);
        Task<DividendIncome> GetDividendIncomeAsync(string userId);
        Task<TopPerformer> GetTopPerformerAsync(string userId);
        
        // Historical Portfolio Data
        Task<List<PortfolioDataPoint>> GetPortfolioHistoryAsync(string userId, int months = 12);
        
        // Health Check
        Task<bool> CheckApiHealthAsync();
    }

    public class PortfolioDataPoint
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Return { get; set; }
    }
}
