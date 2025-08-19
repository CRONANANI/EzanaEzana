using EzanaEzana.Models;
using EzanaEzana.ViewModels;

namespace EzanaEzana.Services
{
    public interface IPlaidService
    {
        // Link token operations
        Task<string> CreateLinkTokenAsync(string userId);
        Task<string> CreateLinkTokenForUpdateAsync(string userId, string accessToken);
        
        // Exchange public token for access token
        Task<string> ExchangePublicTokenAsync(string publicToken);
        
        // Account operations
        Task<List<PlaidAccount>> GetAccountsAsync(string accessToken);
        Task<List<PlaidTransaction>> GetTransactionsAsync(string accessToken, string accountId, DateTime startDate, DateTime endDate);
        
        // Institution operations
        Task<PlaidInstitution> GetInstitutionAsync(string institutionId);
        
        // Webhook handling
        Task ProcessWebhookAsync(string webhookType, string itemId, string webhookCode);
        
        // Data sync
        Task SyncAccountsAsync(string userId);
        Task SyncTransactionsAsync(string userId, DateTime startDate, DateTime endDate);
        
        // User account management
        Task<List<PlaidItem>> GetUserItemsAsync(string userId);
        Task<bool> RemoveItemAsync(string userId, string itemId);
        
        // Health check
        Task<bool> IsHealthyAsync();
    }
}
