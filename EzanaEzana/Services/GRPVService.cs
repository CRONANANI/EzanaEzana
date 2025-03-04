using Ezana.Data;
using Ezana.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ezana.Services
{
    public class GRPVService : IGRPVService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _apiKey;

        public GRPVService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _apiKey = configuration["NasdaqApiKey"] ?? "demo"; // Use demo key if not configured
        }

        public async Task<GRPVModel> CalculateGRPVScores(string stockSymbol, string userId)
        {
            // Check if we already have a recent calculation for this stock and user
            var existingModel = await _context.GRPVModels
                .FirstOrDefaultAsync(m => m.StockSymbol == stockSymbol && m.UserId == userId && m.UpdatedAt > DateTime.UtcNow.AddDays(-1));

            if (existingModel != null)
            {
                return existingModel;
            }

            // Create a new model
            var model = new GRPVModel
            {
                UserId = userId,
                StockSymbol = stockSymbol,
                CompanyName = await GetCompanyName(stockSymbol),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Calculate Growth Score
            model.GrowthFactors = await CalculateGrowthFactors(stockSymbol);
            model.GrowthScore = CalculateGrowthScore(model.GrowthFactors);

            // Calculate Risk Score
            model.RiskFactors = await CalculateRiskFactors(stockSymbol);
            model.RiskScore = CalculateRiskScore(model.RiskFactors);

            // Calculate Profitability Score
            model.ProfitabilityFactors = await CalculateProfitabilityFactors(stockSymbol);
            model.ProfitabilityScore = CalculateProfitabilityScore(model.ProfitabilityFactors);

            // Calculate Valuation Score
            model.ValuationFactors = await CalculateValuationFactors(stockSymbol);
            model.ValuationScore = CalculateValuationScore(model.ValuationFactors);

            // Calculate Overall Score
            model.OverallScore = CalculateOverallScore(model);

            // Save to database
            _context.GRPVModels.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<List<GRPVModel>> GetUserGRPVModels(string userId)
        {
            return await _context.GRPVModels
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.UpdatedAt)
                .ToListAsync();
        }

        public async Task<GRPVModel> GetGRPVModelById(int id, string userId)
        {
            return await _context.GRPVModels
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }

        public async Task<GRPVModel> SaveGRPVModel(GRPVModel model)
        {
            model.UpdatedAt = DateTime.UtcNow;

            if (model.Id == 0)
            {
                model.CreatedAt = DateTime.UtcNow;
                _context.GRPVModels.Add(model);
            }
            else
            {
                _context.GRPVModels.Update(model);
            }

            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteGRPVModel(int id, string userId)
        {
            var model = await _context.GRPVModels
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (model == null)
            {
                return false;
            }

            _context.GRPVModels.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, string>> SearchStocks(string query)
        {
            // In a real implementation, this would call a stock API to search for stocks
            // For demo purposes, we'll return some sample stocks
            await Task.Delay(100); // Simulate API call

            var results = new Dictionary<string, string>
            {
                { "AAPL", "Apple Inc." },
                { "MSFT", "Microsoft Corporation" },
                { "AMZN", "Amazon.com Inc." },
                { "GOOGL", "Alphabet Inc." },
                { "META", "Meta Platforms Inc." },
                { "TSLA", "Tesla Inc." },
                { "NVDA", "NVIDIA Corporation" },
                { "JPM", "JPMorgan Chase & Co." },
                { "V", "Visa Inc." },
                { "JNJ", "Johnson & Johnson" }
            };

            // Filter results based on query
            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToUpper();
                results = results
                    .Where(r => r.Key.Contains(query) || r.Value.ToUpper().Contains(query))
                    .ToDictionary(r => r.Key, r => r.Value);
            }

            return results;
        }

        private async Task<string> GetCompanyName(string stockSymbol)
        {
            // In a real implementation, this would call a stock API to get the company name
            // For demo purposes, we'll use a hardcoded mapping
            var stockNames = new Dictionary<string, string>
            {
                { "AAPL", "Apple Inc." },
                { "MSFT", "Microsoft Corporation" },
                { "AMZN", "Amazon.com Inc." },
                { "GOOGL", "Alphabet Inc." },
                { "META", "Meta Platforms Inc." },
                { "TSLA", "Tesla Inc." },
                { "NVDA", "NVIDIA Corporation" },
                { "JPM", "JPMorgan Chase & Co." },
                { "V", "Visa Inc." },
                { "JNJ", "Johnson & Johnson" }
            };

            await Task.Delay(100); // Simulate API call

            return stockNames.TryGetValue(stockSymbol, out var name) ? name : $"{stockSymbol} Inc.";
        }

        private async Task<Dictionary<string, decimal>> CalculateGrowthFactors(string stockSymbol)
        {
            // In a real implementation, this would call a stock API to get quarterly revenue growth
            // For demo purposes, we'll generate random data
            await Task.Delay(100); // Simulate API call

            var random = new Random();
            var factors = new Dictionary<string, decimal>();

            // Quarterly revenue growth for the past two years (8 quarters)
            for (int i = 1; i <= 8; i++)
            {
                var growthRate = (decimal)(random.NextDouble() * 0.3 - 0.05); // Between -5% and 25%
                factors[$"Q{i} Revenue Growth"] = growthRate;
            }

            return factors;
        }

        private decimal CalculateGrowthScore(Dictionary<string, decimal> factors)
        {
            // Calculate growth score based on quarterly revenue growth
            // Higher growth rates result in a higher score
            
            if (factors.Count == 0)
                return 0;

            // Calculate average growth rate
            var avgGrowthRate = factors.Values.Average();
            
            // Convert to a 0-100 score
            // -10% growth or less = 0, 30% growth or more = 100
            var score = (avgGrowthRate + 0.1m) / 0.4m * 100;
            
            // Clamp between 0 and 100
            return Math.Max(0, Math.Min(100, score));
        }

        private async Task<Dictionary<string, decimal>> CalculateRiskFactors(string stockSymbol)
        {
            // In a real implementation, this would call a stock API to get risk factors
            // For demo purposes, we'll generate random data
            await Task.Delay(100); // Simulate API call

            var random = new Random();
            var factors = new Dictionary<string, decimal>();

            // Debt to equity ratio for the past 5 years
            for (int i = 1; i <= 5; i++)
            {
                var debtToEquity = (decimal)(random.NextDouble() * 2); // Between 0 and 2
                factors[$"Year {i} Debt/Equity"] = debtToEquity;
            }

            // Stock beta
            factors["Beta"] = (decimal)(random.NextDouble() * 2.5); // Between 0 and 2.5

            return factors;
        }

        private decimal CalculateRiskScore(Dictionary<string, decimal> factors)
        {
            if (factors.Count <= 1) // Need at least debt/equity and beta
                return 50; // Default to medium risk

            // Calculate average debt to equity ratio
            var debtToEquityValues = factors.Where(f => f.Key.Contains("Debt/Equity")).Select(f => f.Value);
            var avgDebtToEquity = debtToEquityValues.Any() ? debtToEquityValues.Average() : 1;

            // Get beta
            var beta = factors.TryGetValue("Beta", out var betaValue) ? betaValue : 1;

            // Calculate risk score (higher values = higher risk)
            // Debt/Equity: 0 = low risk, 2+ = high risk
            // Beta: <0.8 = low risk, >1.5 = high risk
            var debtToEquityScore = Math.Min(100, avgDebtToEquity * 50);
            var betaScore = Math.Min(100, beta * 66.67m);

            // Combine scores (60% beta, 40% debt/equity)
            var combinedScore = (betaScore * 0.6m) + (debtToEquityScore * 0.4m);

            // Invert score (higher risk = lower score)
            return 100 - combinedScore;
        }

        private async Task<Dictionary<string, decimal>> CalculateProfitabilityFactors(string stockSymbol)
        {
            // In a real implementation, this would call a stock API to get profitability factors
            // For demo purposes, we'll generate random data
            await Task.Delay(100); // Simulate API call

            var random = new Random();
            var factors = new Dictionary<string, decimal>();

            // Profit margin for the past three years
            for (int i = 1; i <= 3; i++)
            {
                var profitMargin = (decimal)(random.NextDouble() * 0.3); // Between 0% and 30%
                factors[$"Year {i} Profit Margin"] = profitMargin;
            }

            // Operating margin for the past three years
            for (int i = 1; i <= 3; i++)
            {
                var operatingMargin = (decimal)(random.NextDouble() * 0.4); // Between 0% and 40%
                factors[$"Year {i} Operating Margin"] = operatingMargin;
            }

            // Annual dividend yield for the past 3 years
            for (int i = 1; i <= 3; i++)
            {
                var dividendYield = (decimal)(random.NextDouble() * 0.05); // Between 0% and 5%
                factors[$"Year {i} Dividend Yield"] = dividendYield;
            }

            // EBITDA/Sales for the past 3 years
            for (int i = 1; i <= 3; i++)
            {
                var ebitdaToSales = (decimal)(random.NextDouble() * 0.5); // Between 0% and 50%
                factors[$"Year {i} EBITDA/Sales"] = ebitdaToSales;
            }

            return factors;
        }

        private decimal CalculateProfitabilityScore(Dictionary<string, decimal> factors)
        {
            if (factors.Count == 0)
                return 0;

            // Calculate average values for each metric
            var profitMarginValues = factors.Where(f => f.Key.Contains("Profit Margin")).Select(f => f.Value);
            var avgProfitMargin = profitMarginValues.Any() ? profitMarginValues.Average() : 0;

            var operatingMarginValues = factors.Where(f => f.Key.Contains("Operating Margin")).Select(f => f.Value);
            var avgOperatingMargin = operatingMarginValues.Any() ? operatingMarginValues.Average() : 0;

            var dividendYieldValues = factors.Where(f => f.Key.Contains("Dividend Yield")).Select(f => f.Value);
            var avgDividendYield = dividendYieldValues.Any() ? dividendYieldValues.Average() : 0;

            var ebitdaToSalesValues = factors.Where(f => f.Key.Contains("EBITDA/Sales")).Select(f => f.Value);
            var avgEbitdaToSales = ebitdaToSalesValues.Any() ? ebitdaToSalesValues.Average() : 0;

            // Calculate scores for each metric (0-100)
            var profitMarginScore = avgProfitMargin * 333.33m; // 30% = 100 points
            var operatingMarginScore = avgOperatingMargin * 250; // 40% = 100 points
            var dividendYieldScore = avgDividendYield * 2000; // 5% = 100 points
            var ebitdaToSalesScore = avgEbitdaToSales * 200; // 50% = 100 points

            // Combine scores with weights
            var combinedScore = (profitMarginScore * 0.3m) +
                               (operatingMarginScore * 0.3m) +
                               (dividendYieldScore * 0.2m) +
                               (ebitdaToSalesScore * 0.2m);

            // Clamp between 0 and 100
            return Math.Max(0, Math.Min(100, combinedScore));
        }

        private async Task<Dictionary<string, decimal>> CalculateValuationFactors(string stockSymbol)
        {
            // In a real implementation, this would call a stock API to get valuation factors
            // For demo purposes, we'll generate random data
            await Task.Delay(100); // Simulate API call

            var random = new Random();
            var factors = new Dictionary<string, decimal>();

            // Market cap relative to industry
            factors["Market Cap Relative"] = (decimal)(random.NextDouble() * 3); // Between 0 and 3x industry average

            // Trailing P/E ratio for the last 3 years
            for (int i = 1; i <= 3; i++)
            {
                var pe = (decimal)(random.NextDouble() * 50 + 5); // Between 5 and 55
                factors[$"Year {i} P/E"] = pe;
            }

            // PEG ratio for the last 3 years
            for (int i = 1; i <= 3; i++)
            {
                var peg = (decimal)(random.NextDouble() * 3 + 0.5); // Between 0.5 and 3.5
                factors[$"Year {i} PEG"] = peg;
            }

            // Price/Book ratio for the last 3 years
            for (int i = 1; i <= 3; i++)
            {
                var pb = (decimal)(random.NextDouble() * 10 + 1); // Between 1 and 11
                factors[$"Year {i} P/B"] = pb;
            }

            // EV/Revenue ratio for the last 3 years
            for (int i = 1; i <= 3; i++)
            {
                var evToRev = (decimal)(random.NextDouble() * 15 + 1); // Between 1 and 16
                factors[$"Year {i} EV/Revenue"] = evToRev;
            }

            // EPS for the last 3 years
            for (int i = 1; i <= 3; i++)
            {
                var eps = (decimal)(random.NextDouble() * 20 - 5); // Between -5 and 15
                factors[$"Year {i} EPS"] = eps;
            }

            return factors;
        }

        private decimal CalculateValuationScore(Dictionary<string, decimal> factors)
        {
            if (factors.Count == 0)
                return 0;

            // Calculate average values for each metric
            var peValues = factors.Where(f => f.Key.Contains("P/E")).Select(f => f.Value);
            var avgPE = peValues.Any() ? peValues.Average() : 20;

            var pegValues = factors.Where(f => f.Key.Contains("PEG")).Select(f => f.Value);
            var avgPEG = pegValues.Any() ? pegValues.Average() : 2;

            var pbValues = factors.Where(f => f.Key.Contains("P/B")).Select(f => f.Value);
            var avgPB = pbValues.Any() ? pbValues.Average() : 5;

            var evToRevValues = factors.Where(f => f.Key.Contains("EV/Revenue")).Select(f => f.Value);
            var avgEVToRev = evToRevValues.Any() ? evToRevValues.Average() : 8;

            var epsValues = factors.Where(f => f.Key.Contains("EPS")).Select(f => f.Value);
            var avgEPS = epsValues.Any() ? epsValues.Average() : 0;

            var marketCapRelative = factors.TryGetValue("Market Cap Relative", out var mcr) ? mcr : 1;

            // Calculate scores for each metric (0-100, lower values are better for most valuation metrics)
            var peScore = Math.Max(0, 100 - (avgPE - 15) * 2.5m); // PE of 15 or less = 100, PE of 55 = 0
            var pegScore = Math.Max(0, 100 - (avgPEG - 1) * 50); // PEG of 1 or less = 100, PEG of 3 = 0
            var pbScore = Math.Max(0, 100 - (avgPB - 1) * 10); // P/B of 1 or less = 100, P/B of 11 = 0
            var evToRevScore = Math.Max(0, 100 - (avgEVToRev - 1) * 6.67m); // EV/Rev of 1 or less = 100, EV/Rev of 16 = 0
            var epsScore = Math.Max(0, Math.Min(100, (avgEPS + 5) * 5)); // EPS of 15 or more = 100, EPS of -5 = 0
            var marketCapScore = Math.Max(0, 100 - (marketCapRelative - 1) * 50); // MCR of 1 or less = 100, MCR of 3 = 0

            // Combine scores with weights
            var combinedScore = (peScore * 0.2m) +
                               (pegScore * 0.2m) +
                               (pbScore * 0.15m) +
                               (evToRevScore * 0.15m) +
                               (epsScore * 0.2m) +
                               (marketCapScore * 0.1m);

            // Clamp between 0 and 100
            return Math.Max(0, Math.Min(100, combinedScore));
        }

        private decimal CalculateOverallScore(GRPVModel model)
        {
            // Calculate overall score with weights
            // Growth: 25%, Risk: 25%, Profitability: 25%, Valuation: 25%
            return (model.GrowthScore * 0.25m) +
                   (model.RiskScore * 0.25m) +
                   (model.ProfitabilityScore * 0.25m) +
                   (model.ValuationScore * 0.25m);
        }
    }
} 