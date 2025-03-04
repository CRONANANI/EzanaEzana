using Ezana.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ezana.Services
{
    public interface IGRPVService
    {
        Task<GRPVModel> CalculateGRPVScores(string stockSymbol, string userId);
        Task<List<GRPVModel>> GetUserGRPVModels(string userId);
        Task<GRPVModel> GetGRPVModelById(int id, string userId);
        Task<GRPVModel> SaveGRPVModel(GRPVModel model);
        Task<bool> DeleteGRPVModel(int id, string userId);
        Task<Dictionary<string, string>> SearchStocks(string query);
    }
} 