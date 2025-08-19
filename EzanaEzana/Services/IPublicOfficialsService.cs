using EzanaEzana.Models.PublicOfficials;

namespace EzanaEzana.Services
{
    public interface IPublicOfficialsService
    {
        // Individual Card Data Retrieval
        Task<HistoricalCongressTradingCard> GetHistoricalCongressTradingCardAsync(bool useMockData = false);
        Task<GovernmentContractsCard> GetGovernmentContractsCardAsync(bool useMockData = false);
        Task<HouseTradingCard> GetHouseTradingCardAsync(bool useMockData = false);
        Task<LobbyingActivityCard> GetLobbyingActivityCardAsync(bool useMockData = false);
        Task<SenatorTradingCard> GetSenatorTradingCardAsync(bool useMockData = false);
        Task<PatentMomentumCard> GetPatentMomentumCardAsync(bool useMockData = false);
        Task<MarketSentimentCard> GetMarketSentimentCardAsync(bool useMockData = false);

        // Comprehensive Summary
        Task<PublicOfficialsSummary> GetPublicOfficialsSummaryAsync(bool useMockData = false);

        // Utility Methods
        Task<bool> HasQuiverApiAccessAsync();
        Task<string> GetQuiverApiKeyAsync();
        Task<bool> RefreshFromQuiverAsync();
        Task<Dictionary<string, object>> GetMockPublicOfficialsData();

        // Health Check
        Task<bool> CheckQuiverApiHealthAsync();
    }
}
