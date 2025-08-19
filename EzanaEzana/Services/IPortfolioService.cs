using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.Services
{
    public interface IPortfolioService
    {
        // Portfolio Management
        Task<List<Portfolio>> GetUserPortfoliosAsync(string userId);
        Task<Portfolio?> GetPortfolioByIdAsync(int portfolioId, string userId);
        Task<Portfolio> CreatePortfolioAsync(CreatePortfolioViewModel model, string userId);
        Task<bool> UpdatePortfolioAsync(UpdatePortfolioViewModel model, string userId);
        Task<bool> DeletePortfolioAsync(int portfolioId, string userId);
        Task<Portfolio?> GetDefaultPortfolioAsync(string userId);

        // Portfolio Holdings
        Task<List<PortfolioHolding>> GetPortfolioHoldingsAsync(int portfolioId, string userId);
        Task<PortfolioHolding?> GetHoldingByIdAsync(int holdingId, string userId);
        Task<PortfolioHolding> AddHoldingAsync(int portfolioId, AddHoldingViewModel model, string userId);
        Task<bool> UpdateHoldingAsync(int holdingId, UpdateHoldingViewModel model, string userId);
        Task<bool> RemoveHoldingAsync(int holdingId, string userId);

        // Portfolio Transactions
        Task<List<PortfolioTransaction>> GetPortfolioTransactionsAsync(int portfolioId, string userId, int page = 1, int pageSize = 20);
        Task<PortfolioTransaction?> GetTransactionByIdAsync(int transactionId, string userId);
        Task<PortfolioTransaction> AddTransactionAsync(int portfolioId, AddTransactionViewModel model, string userId);
        Task<bool> UpdateTransactionAsync(int transactionId, UpdateTransactionViewModel model, string userId);
        Task<bool> DeleteTransactionAsync(int transactionId, string userId);

        // Portfolio Analytics
        Task<PortfolioSummaryViewModel> GetPortfolioSummaryAsync(int portfolioId, string userId);
        Task<PortfolioPerformanceViewModel> GetPortfolioPerformanceAsync(int portfolioId, string userId, int months = 12);
        Task<PortfolioAllocationViewModel> GetPortfolioAllocationAsync(int portfolioId, string userId);
        Task<List<PortfolioHolding>> GetTopHoldingsAsync(int portfolioId, string userId, int count = 10);

        // Watchlist Management
        Task<List<Watchlist>> GetUserWatchlistsAsync(string userId);
        Task<Watchlist?> GetWatchlistByIdAsync(int watchlistId, string userId);
        Task<Watchlist> CreateWatchlistAsync(CreateWatchlistViewModel model, string userId);
        Task<bool> UpdateWatchlistAsync(int watchlistId, UpdateWatchlistViewModel model, string userId);
        Task<bool> DeleteWatchlistAsync(int watchlistId, string userId);

        // Watchlist Items
        Task<List<WatchlistItem>> GetWatchlistItemsAsync(int watchlistId, string userId);
        Task<WatchlistItem> AddWatchlistItemAsync(int watchlistId, AddWatchlistItemViewModel model, string userId);
        Task<bool> UpdateWatchlistItemAsync(int itemId, UpdateWatchlistItemViewModel model, string userId);
        Task<bool> RemoveWatchlistItemAsync(int itemId, string userId);

        // Portfolio Updates
        Task UpdatePortfolioPricesAsync(int portfolioId, string userId);
        Task<decimal> GetTotalPortfolioValueAsync(int portfolioId, string userId);
        Task<decimal> GetTotalGainLossAsync(int portfolioId, string userId);

        // User Activity Logging
        Task LogUserActivityAsync(string userId, string category, string action, string details);
    }
}
