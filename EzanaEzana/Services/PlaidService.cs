using EzanaEzana.Data;
using EzanaEzana.Models;
using EzanaEzana.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EzanaEzana.Services
{
    public class PlaidService : IPlaidService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PlaidService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _environment;
        private readonly string _baseUrl;

        public PlaidService(
            IConfiguration configuration,
            ILogger<PlaidService> logger,
            ApplicationDbContext context,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _httpClient = httpClient;
            
            // Initialize Plaid configuration
            _clientId = _configuration["Plaid:ClientId"] ?? "test_client_id";
            _secret = _configuration["Plaid:Secret"] ?? "test_secret";
            _environment = _configuration["Plaid:Environment"] ?? "sandbox";
            _baseUrl = GetPlaidBasePath(_environment);
        }

        private string GetPlaidBasePath(string environment) => environment.ToLower() switch
        {
            "sandbox" => "https://sandbox.plaid.com",
            "development" => "https://development.plaid.com",
            "production" => "https://production.plaid.com",
            _ => "https://sandbox.plaid.com"
        };

        public async Task<string> CreateLinkTokenAsync(string userId)
        {
            try
            {
                // For MVP demo, return a mock link token
                // In production, this would make a real API call to Plaid
                var mockToken = $"link_token_{userId}_{Guid.NewGuid():N}";
                
                _logger.LogInformation("Created mock link token for user {UserId}: {Token}", userId, mockToken);
                
                return mockToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating link token for user {UserId}", userId);
                throw;
            }
        }

        public async Task<string> CreateLinkTokenForUpdateAsync(string userId, string accessToken)
        {
            try
            {
                // For MVP demo, return a mock update link token
                var mockToken = $"update_token_{userId}_{Guid.NewGuid():N}";
                
                _logger.LogInformation("Created mock update link token for user {UserId}: {Token}", userId, mockToken);
                
                return mockToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating update link token for user {UserId}", userId);
                throw;
            }
        }

        public async Task<string> ExchangePublicTokenAsync(string publicToken)
        {
            try
            {
                // For MVP demo, return a mock access token
                // In production, this would exchange the public token with Plaid
                var mockAccessToken = $"access_token_{Guid.NewGuid():N}";
                
                _logger.LogInformation("Exchanged public token for mock access token: {AccessToken}", mockAccessToken);
                
                return mockAccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exchanging public token");
                throw;
            }
        }

        public async Task<List<PlaidAccount>> GetAccountsAsync(string accessToken)
        {
            try
            {
                // For MVP demo, return mock accounts
                // In production, this would fetch real accounts from Plaid
                var mockAccounts = new List<PlaidAccount>
                {
                    new PlaidAccount
                    {
                        PlaidAccountId = $"acc_{Guid.NewGuid():N}",
                        PlaidItemId = $"item_{Guid.NewGuid():N}",
                        AccountName = "Demo Checking Account",
                        AccountType = "depository",
                        AccountSubtype = "checking",
                        Mask = "1234",
                        CurrentBalance = 5420.50m,
                        CurrencyCode = "USD",
                        LastUpdated = DateTime.UtcNow
                    },
                    new PlaidAccount
                    {
                        PlaidAccountId = $"acc_{Guid.NewGuid():N}",
                        PlaidItemId = $"item_{Guid.NewGuid():N}",
                        AccountName = "Demo Investment Account",
                        AccountType = "investment",
                        AccountSubtype = "401k",
                        Mask = "5678",
                        CurrentBalance = 125000.00m,
                        CurrencyCode = "USD",
                        LastUpdated = DateTime.UtcNow
                    }
                };
                
                _logger.LogInformation("Retrieved {Count} mock accounts for access token", mockAccounts.Count);
                
                return mockAccounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts for access token");
                throw;
            }
        }

        public async Task<List<PlaidTransaction>> GetTransactionsAsync(string accessToken, string accountId, DateTime startDate, DateTime endDate)
        {
            try
            {
                // For MVP demo, return mock transactions
                // In production, this would fetch real transactions from Plaid
                var mockTransactions = new List<PlaidTransaction>
                {
                    new PlaidTransaction
                    {
                        PlaidTransactionId = $"txn_{Guid.NewGuid():N}",
                        PlaidAccountId = accountId,
                        Name = "Grocery Store Purchase",
                        Amount = -85.50m,
                        CurrencyCode = "USD",
                        Date = DateTime.UtcNow.AddDays(-1),
                        Category = "Food and Drink",
                        CategoryId = "22016000",
                        MerchantName = "Whole Foods Market",
                        PaymentChannel = "in store",
                        Pending = false,
                        LastUpdated = DateTime.UtcNow
                    },
                    new PlaidTransaction
                    {
                        PlaidTransactionId = $"txn_{Guid.NewGuid():N}",
                        PlaidAccountId = accountId,
                        Name = "Salary Deposit",
                        Amount = 3500.00m,
                        CurrencyCode = "USD",
                        Date = DateTime.UtcNow.AddDays(-7),
                        Category = "Transfer",
                        CategoryId = "21000000",
                        MerchantName = "Employer Inc",
                        PaymentChannel = "online",
                        Pending = false,
                        LastUpdated = DateTime.UtcNow
                    }
                };
                
                _logger.LogInformation("Retrieved {Count} mock transactions for account {AccountId}", mockTransactions.Count, accountId);
                
                return mockTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions for account {AccountId}", accountId);
                throw;
            }
        }

        public async Task<PlaidInstitution> GetInstitutionAsync(string institutionId)
        {
            try
            {
                // For MVP demo, return mock institution
                // In production, this would fetch real institution data from Plaid
                var mockInstitution = new PlaidInstitution
                {
                    PlaidInstitutionId = institutionId,
                    Name = "Demo Bank",
                    Logo = "https://example.com/logo.png",
                    PrimaryColor = "#1a73e8",
                    Url = "https://demobank.com",
                    HasOAuth = false,
                    LastUpdated = DateTime.UtcNow
                };
                
                _logger.LogInformation("Retrieved mock institution: {InstitutionName}", mockInstitution.Name);
                
                return mockInstitution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting institution {InstitutionId}", institutionId);
                throw;
            }
        }

        public async Task ProcessWebhookAsync(string webhookType, string itemId, string webhookCode)
        {
            try
            {
                _logger.LogInformation("Processing webhook: {WebhookType} - {WebhookCode} for item {ItemId}", 
                    webhookType, webhookCode, itemId);

                switch (webhookCode)
                {
                    case "ITEM_LOGIN_REQUIRED":
                        await HandleItemLoginRequiredAsync(itemId);
                        break;
                    case "ITEM_ERROR":
                        await HandleItemErrorAsync(itemId);
                        break;
                    case "TRANSACTIONS_REMOVED":
                        await HandleTransactionsRemovedAsync(itemId);
                        break;
                    case "DEFAULT_UPDATE":
                        await HandleDefaultUpdateAsync(itemId);
                        break;
                    default:
                        _logger.LogWarning("Unknown webhook code: {WebhookCode}", webhookCode);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing webhook {WebhookType} - {WebhookCode}", webhookType, webhookCode);
                throw;
            }
        }

        private async Task HandleItemLoginRequiredAsync(string itemId)
        {
            var item = await _context.PlaidItems
                .FirstOrDefaultAsync(i => i.PlaidItemId == itemId);
            
            if (item != null)
            {
                item.IsActive = false;
                item.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private async Task HandleItemErrorAsync(string itemId)
        {
            var item = await _context.PlaidItems
                .FirstOrDefaultAsync(i => i.PlaidItemId == itemId);
            
            if (item != null)
            {
                item.IsActive = false;
                item.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private async Task HandleTransactionsRemovedAsync(string itemId)
        {
            // Handle removed transactions
            _logger.LogInformation("Transactions removed for item {ItemId}", itemId);
        }

        private async Task HandleDefaultUpdateAsync(string itemId)
        {
            // Handle default updates
            _logger.LogInformation("Default update for item {ItemId}", itemId);
        }

        public async Task SyncAccountsAsync(string userId)
        {
            try
            {
                var userItems = await _context.PlaidItems
                    .Where(i => i.UserId == userId && i.IsActive)
                    .ToListAsync();

                foreach (var item in userItems)
                {
                    var accounts = await GetAccountsAsync(item.PlaidAccessToken);
                    
                    foreach (var account in accounts)
                    {
                        account.UserId = userId;
                        account.Id = Guid.NewGuid().ToString();
                        
                        var existingAccount = await _context.PlaidAccounts
                            .FirstOrDefaultAsync(a => a.PlaidAccountId == account.PlaidAccountId);
                        
                        if (existingAccount != null)
                        {
                            // Update existing account
                            existingAccount.CurrentBalance = account.CurrentBalance;
                            existingAccount.LastUpdated = DateTime.UtcNow;
                        }
                        else
                        {
                            // Add new account
                            _context.PlaidAccounts.Add(account);
                        }
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing accounts for user {UserId}", userId);
                throw;
            }
        }

        public async Task SyncTransactionsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userItems = await _context.PlaidItems
                    .Where(i => i.UserId == userId && i.IsActive)
                    .ToListAsync();

                foreach (var item in userItems)
                {
                    var accounts = await _context.PlaidAccounts
                        .Where(a => a.PlaidItemId == item.PlaidItemId)
                        .ToListAsync();

                    foreach (var account in accounts)
                    {
                        var transactions = await GetTransactionsAsync(
                            item.PlaidAccessToken, 
                            account.PlaidAccountId, 
                            startDate, 
                            endDate);

                        foreach (var transaction in transactions)
                        {
                            transaction.AccountId = account.Id;
                            transaction.Id = Guid.NewGuid().ToString();
                            
                            var existingTransaction = await _context.PlaidTransactions
                                .FirstOrDefaultAsync(t => t.PlaidTransactionId == transaction.PlaidTransactionId);
                            
                            if (existingTransaction == null)
                            {
                                _context.PlaidTransactions.Add(transaction);
                            }
                        }
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing transactions for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<PlaidItem>> GetUserItemsAsync(string userId)
        {
            return await _context.PlaidItems
                .Where(i => i.UserId == userId && i.IsActive)
                .Include(i => i.Accounts)
                .ToListAsync();
        }

        public async Task<bool> RemoveItemAsync(string userId, string itemId)
        {
            try
            {
                var item = await _context.PlaidItems
                    .FirstOrDefaultAsync(i => i.Id == itemId && i.UserId == userId);
                
                if (item != null)
                {
                    item.IsActive = false;
                    item.LastUpdated = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ItemId} for user {UserId}", itemId, userId);
                return false;
            }
        }

        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                // For MVP demo, always return healthy
                // In production, this would check Plaid API health
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
