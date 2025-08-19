using System.Collections.Generic;
using System.Threading.Tasks;
using EzanaEzana.Models;

namespace EzanaEzana.Services
{
    public interface IInvestmentAnalyticsService
    {
        Task<Dictionary<string, object>> GetInvestmentRecommendations(string userId);
        Task<Dictionary<string, object>> GetPortfolioAllocation(string userId);
    }
} 