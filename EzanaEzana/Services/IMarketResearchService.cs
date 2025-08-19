using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EzanaEzana.Services
{
    public interface IMarketResearchService
    {
        // Market Analysis
        Task<IEnumerable<MarketData>> GetMarketDataAsync(string? symbol = null, int page = 1, int pageSize = 50);
        Task<MarketData?> GetMarketDataBySymbolAsync(string symbol);
        Task<IEnumerable<MarketDataHistory>> GetMarketDataHistoryAsync(string symbol, DateTime startDate, DateTime endDate);
        Task<CompanyProfile?> GetCompanyProfileAsync(string symbol);
        Task<IEnumerable<CompanyFinancial>> GetCompanyFinancialsAsync(string symbol, int page = 1, int pageSize = 20);
        Task<IEnumerable<CompanyNews>> GetCompanyNewsAsync(string symbol, int page = 1, int pageSize = 20);
        Task<IEnumerable<EconomicIndicator>> GetEconomicIndicatorsAsync(string? category = null, int page = 1, int pageSize = 20);
        Task<IEnumerable<EconomicIndicatorHistory>> GetEconomicIndicatorHistoryAsync(int indicatorId, DateTime startDate, DateTime endDate);
        
        // Market Intelligence
        Task<MarketIntelligenceViewModel> GetMarketIntelligenceAsync();
        Task<EconomicOutlookViewModel> GetEconomicOutlookAsync();
        Task<MarketTrendAnalysisViewModel> AnalyzeMarketTrendsAsync(string symbol, DateTime startDate, DateTime endDate);
        Task<CompanyComparisonViewModel> CompareCompaniesAsync(List<string> symbols);
        
        // Data Updates
        Task<bool> UpdateMarketDataAsync(string symbol, decimal currentPrice, decimal priceChange, decimal priceChangePercent, long volume);
        Task<bool> LogUserActivityAsync(string userId, string category, string action, string details);

        #region Quiver API Integration Methods
        Task<IEnumerable<CongressionalTrading>> GetCongressionalTradingAsync(int limit = 100);
        Task<IEnumerable<GovernmentContract>> GetGovernmentContractsAsync(int limit = 100);
        Task<IEnumerable<HouseTrading>> GetHouseTradingAsync(int limit = 100);
        Task<IEnumerable<SenatorTrading>> GetSenatorTradingAsync(int limit = 100);
        Task<IEnumerable<LobbyingActivity>> GetLobbyingActivityAsync(int limit = 100);
        Task<IEnumerable<PatentMomentum>> GetPatentMomentumAsync(int limit = 100);
        Task<Dictionary<string, object>> GetMarketIntelligenceSummaryAsync();
        #endregion
    }
}
