using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EzanaEzana.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfolioService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PortfolioService(
            ApplicationDbContext context, 
            ILogger<PortfolioService> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Portfolio>> GetUserPortfoliosAsync(string userId)
        {
            try
            {
                return await _context.Portfolios
                    .Include(p => p.Holdings)
                    .Where(p => p.UserId == userId && p.IsActive)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolios for user {UserId}", userId);
                return new List<Portfolio>();
            }
        }

        public async Task<Portfolio?> GetPortfolioByIdAsync(int portfolioId, string userId)
        {
            try
            {
                return await _context.Portfolios
                    .Include(p => p.Holdings)
                    .Include(p => p.Transactions)
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio {PortfolioId} for user {UserId}", portfolioId, userId);
                return null;
            }
        }

        public async Task<Portfolio> CreatePortfolioAsync(CreatePortfolioViewModel model, string userId)
        {
            try
            {
                var portfolio = new Portfolio
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserId = userId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Portfolios.Add(portfolio);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Created portfolio", portfolio.Id.ToString());

                _logger.LogInformation("Portfolio {PortfolioName} created by user {UserId}", 
                    model.Name, userId);

                return portfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating portfolio for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> UpdatePortfolioAsync(UpdatePortfolioViewModel model, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .FirstOrDefaultAsync(p => p.Id == model.Id && p.UserId == userId);

                if (portfolio == null)
                {
                    return false;
                }

                portfolio.Name = model.Name;
                portfolio.Description = model.Description;
                portfolio.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Updated portfolio", portfolio.Id.ToString());

                _logger.LogInformation("Portfolio {PortfolioId} updated by user {UserId}", 
                    model.Id, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating portfolio {PortfolioId} by user {UserId}", 
                    model.Id, userId);
                return false;
            }
        }

        public async Task<bool> DeletePortfolioAsync(int portfolioId, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    return false;
                }

                portfolio.IsActive = false;
                portfolio.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Deleted portfolio", portfolioId.ToString());

                _logger.LogInformation("Portfolio {PortfolioId} deleted by user {UserId}", 
                    portfolioId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting portfolio {PortfolioId} by user {UserId}", 
                    portfolioId, userId);
                return false;
            }
        }

        public async Task<PortfolioHolding> AddHoldingAsync(AddHoldingViewModel model, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .FirstOrDefaultAsync(p => p.Id == model.PortfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    throw new InvalidOperationException("Portfolio not found or access denied");
                }

                var holding = new PortfolioHolding
                {
                    PortfolioId = model.PortfolioId,
                    Symbol = model.Symbol.ToUpper(),
                    CompanyName = model.CompanyName,
                    Shares = model.Shares,
                    AveragePrice = model.AveragePrice,
                    CurrentPrice = model.CurrentPrice,
                    CurrentValue = model.Shares * model.CurrentPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.PortfolioHoldings.Add(holding);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Added holding", $"{model.Symbol} to portfolio {model.PortfolioId}");

                _logger.LogInformation("Holding {Symbol} added to portfolio {PortfolioId} by user {UserId}", 
                    model.Symbol, model.PortfolioId, userId);

                return holding;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding holding to portfolio {PortfolioId} by user {UserId}", 
                    model.PortfolioId, userId);
                throw;
            }
        }

        public async Task<bool> UpdateHoldingAsync(UpdateHoldingViewModel model, string userId)
        {
            try
            {
                var holding = await _context.PortfolioHoldings
                    .Include(ph => ph.Portfolio)
                    .FirstOrDefaultAsync(ph => ph.Id == model.Id && ph.Portfolio.UserId == userId);

                if (holding == null)
                {
                    return false;
                }

                holding.Shares = model.Shares;
                holding.AveragePrice = model.AveragePrice;
                holding.CurrentPrice = model.CurrentPrice;
                holding.CurrentValue = model.Shares * model.CurrentPrice;
                holding.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Updated holding", $"{holding.Symbol} in portfolio {holding.PortfolioId}");

                _logger.LogInformation("Holding {HoldingId} updated by user {UserId}", 
                    model.Id, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating holding {HoldingId} by user {UserId}", 
                    model.Id, userId);
                return false;
            }
        }

        public async Task<bool> RemoveHoldingAsync(int holdingId, string userId)
        {
            try
            {
                var holding = await _context.PortfolioHoldings
                    .Include(ph => ph.Portfolio)
                    .FirstOrDefaultAsync(ph => ph.Id == holdingId && ph.Portfolio.UserId == userId);

                if (holding == null)
                {
                    return false;
                }

                _context.PortfolioHoldings.Remove(holding);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Removed holding", $"{holding.Symbol} from portfolio {holding.PortfolioId}");

                _logger.LogInformation("Holding {HoldingId} removed by user {UserId}", 
                    holdingId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing holding {HoldingId} by user {UserId}", 
                    holdingId, userId);
                return false;
            }
        }

        public async Task<PortfolioTransaction> AddTransactionAsync(AddTransactionViewModel model, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .FirstOrDefaultAsync(p => p.Id == model.PortfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    throw new InvalidOperationException("Portfolio not found or access denied");
                }

                var transaction = new PortfolioTransaction
                {
                    PortfolioId = model.PortfolioId,
                    Symbol = model.Symbol.ToUpper(),
                    TransactionType = model.TransactionType,
                    Shares = model.Shares,
                    Price = model.Price,
                    TotalAmount = model.Shares * model.Price,
                    TransactionDate = model.TransactionDate,
                    Notes = model.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PortfolioTransactions.Add(transaction);

                // Update or create holding
                var existingHolding = await _context.PortfolioHoldings
                    .FirstOrDefaultAsync(ph => ph.PortfolioId == model.PortfolioId && ph.Symbol == model.Symbol);

                if (existingHolding != null)
                {
                    if (model.TransactionType == TransactionType.Buy)
                    {
                        var totalShares = existingHolding.Shares + model.Shares;
                        var totalCost = (existingHolding.Shares * existingHolding.AveragePrice) + (model.Shares * model.Price);
                        existingHolding.Shares = totalShares;
                        existingHolding.AveragePrice = totalCost / totalShares;
                    }
                    else if (model.TransactionType == TransactionType.Sell)
                    {
                        existingHolding.Shares -= model.Shares;
                        if (existingHolding.Shares <= 0)
                        {
                            _context.PortfolioHoldings.Remove(existingHolding);
                        }
                    }
                }
                else if (model.TransactionType == TransactionType.Buy)
                {
                    var newHolding = new PortfolioHolding
                    {
                        PortfolioId = model.PortfolioId,
                        Symbol = model.Symbol.ToUpper(),
                        CompanyName = model.CompanyName ?? model.Symbol,
                        Shares = model.Shares,
                        AveragePrice = model.Price,
                        CurrentPrice = model.Price,
                        CurrentValue = model.Shares * model.Price,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.PortfolioHoldings.Add(newHolding);
                }

                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Added transaction", $"{model.TransactionType} {model.Shares} {model.Symbol}");

                _logger.LogInformation("Transaction added to portfolio {PortfolioId} by user {UserId}", 
                    model.PortfolioId, userId);

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding transaction to portfolio {PortfolioId} by user {UserId}", 
                    model.PortfolioId, userId);
                throw;
            }
        }

        public async Task<IEnumerable<PortfolioTransaction>> GetPortfolioTransactionsAsync(int portfolioId, string userId, int page = 1, int pageSize = 50)
        {
            try
            {
                return await _context.PortfolioTransactions
                    .Include(pt => pt.Portfolio)
                    .Where(pt => pt.PortfolioId == portfolioId && pt.Portfolio.UserId == userId)
                    .OrderByDescending(pt => pt.TransactionDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions for portfolio {PortfolioId}", portfolioId);
                return new List<PortfolioTransaction>();
            }
        }

        public async Task<PortfolioSummaryViewModel> GetPortfolioSummaryAsync(int portfolioId, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .Include(p => p.Holdings)
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    return new PortfolioSummaryViewModel();
                }

                var totalValue = portfolio.Holdings.Sum(h => h.CurrentValue);
                var totalCost = portfolio.Holdings.Sum(h => h.Shares * h.AveragePrice);
                var totalGainLoss = totalValue - totalCost;
                var totalGainLossPercent = totalCost > 0 ? (totalGainLoss / totalCost) * 100 : 0;

                var holdings = portfolio.Holdings.Select(h => new HoldingSummaryViewModel
                {
                    Id = h.Id,
                    Symbol = h.Symbol,
                    CompanyName = h.CompanyName,
                    Shares = h.Shares,
                    AveragePrice = h.AveragePrice,
                    CurrentPrice = h.CurrentPrice,
                    CurrentValue = h.CurrentValue,
                    GainLoss = h.CurrentValue - (h.Shares * h.AveragePrice),
                    GainLossPercent = h.AveragePrice > 0 ? ((h.CurrentPrice - h.AveragePrice) / h.AveragePrice) * 100 : 0
                }).ToList();

                return new PortfolioSummaryViewModel
                {
                    PortfolioId = portfolio.Id,
                    PortfolioName = portfolio.Name,
                    TotalValue = totalValue,
                    TotalCost = totalCost,
                    TotalGainLoss = totalGainLoss,
                    TotalGainLossPercent = totalGainLossPercent,
                    HoldingsCount = holdings.Count,
                    Holdings = holdings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio summary for portfolio {PortfolioId}", portfolioId);
                return new PortfolioSummaryViewModel();
            }
        }

        public async Task<PortfolioPerformanceViewModel> GetPortfolioPerformanceAsync(int portfolioId, string userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .Include(p => p.Holdings)
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    return new PortfolioPerformanceViewModel();
                }

                var transactions = await _context.PortfolioTransactions
                    .Where(pt => pt.PortfolioId == portfolioId && 
                                pt.TransactionDate >= startDate && 
                                pt.TransactionDate <= endDate)
                    .OrderBy(pt => pt.TransactionDate)
                    .ToListAsync();

                var performanceData = new List<PerformanceDataPoint>();
                var currentDate = startDate.Date;

                while (currentDate <= endDate.Date)
                {
                    var holdingsAtDate = await CalculateHoldingsAtDateAsync(portfolioId, currentDate);
                    var totalValue = holdingsAtDate.Sum(h => h.CurrentValue);

                    performanceData.Add(new PerformanceDataPoint
                    {
                        Date = currentDate,
                        TotalValue = totalValue
                    });

                    currentDate = currentDate.AddDays(1);
                }

                return new PortfolioPerformanceViewModel
                {
                    PortfolioId = portfolio.Id,
                    PortfolioName = portfolio.Name,
                    StartDate = startDate,
                    EndDate = endDate,
                    PerformanceData = performanceData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio performance for portfolio {PortfolioId}", portfolioId);
                return new PortfolioPerformanceViewModel();
            }
        }

        public async Task<PortfolioAllocationViewModel> GetPortfolioAllocationAsync(int portfolioId, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .Include(p => p.Holdings)
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    return new PortfolioAllocationViewModel();
                }

                var totalValue = portfolio.Holdings.Sum(h => h.CurrentValue);

                var allocation = portfolio.Holdings
                    .GroupBy(h => h.Symbol)
                    .Select(g => new AllocationItemViewModel
                    {
                        Symbol = g.Key,
                        CompanyName = g.First().CompanyName,
                        Shares = g.Sum(h => h.Shares),
                        CurrentValue = g.Sum(h => h.CurrentValue),
                        Percentage = totalValue > 0 ? (g.Sum(h => h.CurrentValue) / totalValue) * 100 : 0
                    })
                    .OrderByDescending(a => a.CurrentValue)
                    .ToList();

                return new PortfolioAllocationViewModel
                {
                    PortfolioId = portfolio.Id,
                    PortfolioName = portfolio.Name,
                    TotalValue = totalValue,
                    Allocation = allocation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio allocation for portfolio {PortfolioId}", portfolioId);
                return new PortfolioAllocationViewModel();
            }
        }

        public async Task<IEnumerable<Watchlist>> GetUserWatchlistsAsync(string userId)
        {
            try
            {
                return await _context.Watchlists
                    .Include(w => w.Items)
                    .Where(w => w.UserId == userId && w.IsActive)
                    .OrderByDescending(w => w.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting watchlists for user {UserId}", userId);
                return new List<Watchlist>();
            }
        }

        public async Task<Watchlist?> GetWatchlistByIdAsync(int watchlistId, string userId)
        {
            try
            {
                return await _context.Watchlists
                    .Include(w => w.Items)
                    .FirstOrDefaultAsync(w => w.Id == watchlistId && w.UserId == userId && w.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting watchlist {WatchlistId} for user {UserId}", watchlistId, userId);
                return null;
            }
        }

        public async Task<Watchlist> CreateWatchlistAsync(CreateWatchlistViewModel model, string userId)
        {
            try
            {
                var watchlist = new Watchlist
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserId = userId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Watchlists.Add(watchlist);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Watchlist", "Created watchlist", watchlist.Id.ToString());

                _logger.LogInformation("Watchlist {WatchlistName} created by user {UserId}", 
                    model.Name, userId);

                return watchlist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating watchlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> AddToWatchlistAsync(int watchlistId, string symbol, string userId)
        {
            try
            {
                var watchlist = await _context.Watchlists
                    .FirstOrDefaultAsync(w => w.Id == watchlistId && w.UserId == userId);

                if (watchlist == null)
                {
                    return false;
                }

                var existingItem = await _context.WatchlistItems
                    .FirstOrDefaultAsync(wi => wi.WatchlistId == watchlistId && wi.Symbol == symbol);

                if (existingItem != null)
                {
                    return false; // Already in watchlist
                }

                var item = new WatchlistItem
                {
                    WatchlistId = watchlistId,
                    Symbol = symbol.ToUpper(),
                    AddedAt = DateTime.UtcNow
                };

                _context.WatchlistItems.Add(item);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Watchlist", "Added symbol", $"{symbol} to watchlist {watchlistId}");

                _logger.LogInformation("Symbol {Symbol} added to watchlist {WatchlistId} by user {UserId}", 
                    symbol, watchlistId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding symbol {Symbol} to watchlist {WatchlistId} by user {UserId}", 
                    symbol, watchlistId, userId);
                return false;
            }
        }

        public async Task<bool> RemoveFromWatchlistAsync(int watchlistId, string symbol, string userId)
        {
            try
            {
                var item = await _context.WatchlistItems
                    .Include(wi => wi.Watchlist)
                    .FirstOrDefaultAsync(wi => wi.WatchlistId == watchlistId && 
                                              wi.Symbol == symbol && 
                                              wi.Watchlist.UserId == userId);

                if (item == null)
                {
                    return false;
                }

                _context.WatchlistItems.Remove(item);
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Watchlist", "Removed symbol", $"{symbol} from watchlist {watchlistId}");

                _logger.LogInformation("Symbol {Symbol} removed from watchlist {WatchlistId} by user {UserId}", 
                    symbol, watchlistId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing symbol {Symbol} from watchlist {WatchlistId} by user {UserId}", 
                    symbol, watchlistId, userId);
                return false;
            }
        }

        public async Task<bool> UpdatePortfolioPricesAsync(int portfolioId, string userId)
        {
            try
            {
                var portfolio = await _context.Portfolios
                    .Include(p => p.Holdings)
                    .FirstOrDefaultAsync(p => p.Id == portfolioId && p.UserId == userId);

                if (portfolio == null)
                {
                    return false;
                }

                // In a real application, this would call an external API to get current prices
                // For now, we'll just update the timestamp
                foreach (var holding in portfolio.Holdings)
                {
                    holding.UpdatedAt = DateTime.UtcNow;
                }

                portfolio.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Log user activity
                await LogUserActivityAsync(userId, "Portfolio", "Updated prices", $"Portfolio {portfolioId}");

                _logger.LogInformation("Portfolio prices updated for portfolio {PortfolioId} by user {UserId}", 
                    portfolioId, userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating portfolio prices for portfolio {PortfolioId} by user {UserId}", 
                    portfolioId, userId);
                return false;
            }
        }

        private async Task<List<PortfolioHolding>> CalculateHoldingsAtDateAsync(int portfolioId, DateTime date)
        {
            // This is a simplified calculation - in a real application, you'd need to track historical prices
            var holdings = await _context.PortfolioHoldings
                .Where(ph => ph.PortfolioId == portfolioId)
                .ToListAsync();

            return holdings;
        }

        private async Task LogUserActivityAsync(string userId, string category, string action, string details)
        {
            try
            {
                var activity = new UserActivity
                {
                    UserId = userId,
                    Category = category,
                    Action = action,
                    Details = details,
                    Timestamp = DateTime.UtcNow
                };

                _context.UserActivities.Add(activity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user activity for user {UserId}", userId);
            }
        }
    }
}
