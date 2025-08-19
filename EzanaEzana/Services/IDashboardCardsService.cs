using EzanaEzana.Models;
using EzanaEzana.Models.DashboardCards;

namespace EzanaEzana.Services
{
    /// <summary>
    /// Service interface for dashboard cards data
    /// </summary>
    public interface IDashboardCardsService
    {
        /// <summary>
        /// Get portfolio value card data
        /// </summary>
        Task<PortfolioValueCard> GetPortfolioValueCardAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Get today's P&L card data
        /// </summary>
        Task<TodaysPnlCard> GetTodaysPnlCardAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Get top performer card data
        /// </summary>
        Task<TopPerformerCard> GetTopPerformerCardAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Get risk score card data
        /// </summary>
        Task<RiskScoreCard> GetRiskScoreCardAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Get monthly dividends card data
        /// </summary>
        Task<MonthlyDividendsCard> GetMonthlyDividendsCardAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Get asset allocation card data
        /// </summary>
        Task<AssetAllocationCard> GetAssetAllocationCardAsync(string userId, string breakdownType = "asset_class", bool useMockData = false);

        /// <summary>
        /// Get comprehensive dashboard summary with all cards
        /// </summary>
        Task<DashboardCardsSummary> GetDashboardSummaryAsync(string userId, bool useMockData = false);

        /// <summary>
        /// Check if user has connected Plaid accounts
        /// </summary>
        Task<bool> HasPlaidAccountsAsync(string userId);

        /// <summary>
        /// Get user's Plaid access tokens
        /// </summary>
        Task<List<string>> GetUserPlaidAccessTokensAsync(string userId);

        /// <summary>
        /// Refresh dashboard data from Plaid API
        /// </summary>
        Task<bool> RefreshFromPlaidAsync(string userId);

        /// <summary>
        /// Get mock data for testing purposes
        /// </summary>
        DashboardCardsSummary GetMockDashboardData();
    }
}
