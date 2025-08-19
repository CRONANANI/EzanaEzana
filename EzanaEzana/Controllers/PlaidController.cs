using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using EzanaEzana.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaidController : ControllerBase
    {
        private readonly IPlaidService _plaidService;
        private readonly ILogger<PlaidController> _logger;
        private readonly ApplicationDbContext _context;

        public PlaidController(
            IPlaidService plaidService,
            ILogger<PlaidController> logger,
            ApplicationDbContext context)
        {
            _plaidService = plaidService;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Create a link token for connecting a new bank account
        /// </summary>
        [HttpPost("link-token")]
        [Authorize]
        public async Task<IActionResult> CreateLinkToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var linkToken = await _plaidService.CreateLinkTokenAsync(userId);
                
                return Ok(new { linkToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating link token");
                return StatusCode(500, new { error = "Failed to create link token" });
            }
        }

        /// <summary>
        /// Create a link token for updating an existing connection
        /// </summary>
        [HttpPost("link-token-update")]
        [Authorize]
        public async Task<IActionResult> CreateUpdateLinkToken([FromBody] UpdateLinkTokenRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var linkToken = await _plaidService.CreateLinkTokenForUpdateAsync(userId, request.AccessToken);
                
                return Ok(new { linkToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating update link token");
                return StatusCode(500, new { error = "Failed to create update link token" });
            }
        }

        /// <summary>
        /// Exchange public token for access token and save account information
        /// </summary>
        [HttpPost("exchange-token")]
        [Authorize]
        public async Task<IActionResult> ExchangeToken([FromBody] ExchangeTokenRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Exchange public token for access token
                var accessToken = await _plaidService.ExchangePublicTokenAsync(request.PublicToken);
                
                // Get accounts from Plaid
                var accounts = await _plaidService.GetAccountsAsync(accessToken);
                
                // Get institution information
                var institution = await _plaidService.GetInstitutionAsync(request.InstitutionId);
                
                // Save Plaid item
                var plaidItem = new PlaidItem
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    PlaidItemId = request.ItemId,
                    PlaidAccessToken = accessToken,
                    InstitutionId = request.InstitutionId,
                    InstitutionName = institution.Name,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    IsActive = true
                };
                
                _context.PlaidItems.Add(plaidItem);
                
                // Save accounts
                foreach (var account in accounts)
                {
                    account.Id = Guid.NewGuid().ToString();
                    account.UserId = userId;
                    account.PlaidItemId = request.ItemId;
                    _context.PlaidAccounts.Add(account);
                }
                
                await _context.SaveChangesAsync();
                
                return Ok(new { 
                    message = "Account connected successfully",
                    itemId = plaidItem.Id,
                    accountCount = accounts.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exchanging token");
                return StatusCode(500, new { error = "Failed to connect account" });
            }
        }

        /// <summary>
        /// Get user's connected accounts
        /// </summary>
        [HttpGet("accounts")]
        [Authorize]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var items = await _plaidService.GetUserItemsAsync(userId);
                
                var result = items.Select(item => new
                {
                    itemId = item.Id,
                    institutionName = item.InstitutionName,
                    accounts = item.Accounts.Select(acc => new
                    {
                        id = acc.Id,
                        name = acc.AccountName,
                        type = acc.AccountType,
                        subtype = acc.AccountSubtype,
                        mask = acc.Mask,
                        currentBalance = acc.CurrentBalance,
                        currencyCode = acc.CurrencyCode,
                        lastUpdated = acc.LastUpdated
                    }).ToList()
                }).ToList();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts");
                return StatusCode(500, new { error = "Failed to retrieve accounts" });
            }
        }

        /// <summary>
        /// Get transactions for a specific account
        /// </summary>
        [HttpGet("accounts/{accountId}/transactions")]
        [Authorize]
        public async Task<IActionResult> GetTransactions(string accountId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Verify user owns this account
                var account = await _context.PlaidAccounts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);
                
                if (account == null)
                {
                    return NotFound();
                }

                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;
                
                var transactions = await _context.PlaidTransactions
                    .Where(t => t.AccountId == accountId && t.Date >= start && t.Date <= end)
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();
                
                var result = transactions.Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    amount = t.Amount,
                    currencyCode = t.CurrencyCode,
                    date = t.Date,
                    category = t.Category,
                    merchantName = t.MerchantName,
                    pending = t.Pending
                }).ToList();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions for account {AccountId}", accountId);
                return StatusCode(500, new { error = "Failed to retrieve transactions" });
            }
        }

        /// <summary>
        /// Sync accounts and transactions from Plaid
        /// </summary>
        [HttpPost("sync")]
        [Authorize]
        public async Task<IActionResult> SyncData()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Sync accounts
                await _plaidService.SyncAccountsAsync(userId);
                
                // Sync transactions for the last 30 days
                var startDate = DateTime.UtcNow.AddDays(-30);
                var endDate = DateTime.UtcNow;
                await _plaidService.SyncTransactionsAsync(userId, startDate, endDate);
                
                return Ok(new { message = "Data synced successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing data");
                return StatusCode(500, new { error = "Failed to sync data" });
            }
        }

        /// <summary>
        /// Remove a connected account
        /// </summary>
        [HttpDelete("items/{itemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(string itemId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var success = await _plaidService.RemoveItemAsync(userId, itemId);
                
                if (success)
                {
                    return Ok(new { message = "Account removed successfully" });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {ItemId}", itemId);
                return StatusCode(500, new { error = "Failed to remove account" });
            }
        }

        /// <summary>
        /// Handle Plaid webhooks
        /// </summary>
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook([FromBody] PlaidWebhookRequest webhook)
        {
            try
            {
                _logger.LogInformation("Received webhook: {WebhookType} - {WebhookCode} for item {ItemId}", 
                    webhook.WebhookType, webhook.WebhookCode, webhook.ItemId);
                
                await _plaidService.ProcessWebhookAsync(webhook.WebhookType, webhook.ItemId, webhook.WebhookCode);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing webhook");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Check Plaid service health
        /// </summary>
        [HttpGet("health")]
        [AllowAnonymous]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var isHealthy = await _plaidService.IsHealthyAsync();
                
                if (isHealthy)
                {
                    return Ok(new { status = "healthy" });
                }
                else
                {
                    return StatusCode(503, new { status = "unhealthy" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Plaid health");
                return StatusCode(503, new { status = "error" });
            }
        }
    }

    public class UpdateLinkTokenRequest
    {
        public string AccessToken { get; set; } = null!;
    }

    public class ExchangeTokenRequest
    {
        public string PublicToken { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string InstitutionId { get; set; } = null!;
    }

    public class PlaidWebhookRequest
    {
        public string WebhookType { get; set; } = null!;
        public string WebhookCode { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string? Error { get; set; }
        public string[]? RemovedTransactions { get; set; }
    }
}
