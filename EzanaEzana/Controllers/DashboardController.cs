using Ezana.Models;
using Ezana.Services;
using Ezana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ezana.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvestmentAnalyticsService _analyticsService;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IInvestmentAnalyticsService analyticsService)
        {
            _userManager = userManager;
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var viewModel = new DashboardViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName ?? "User",
                InvestmentRecommendations = await _analyticsService.GetInvestmentRecommendations(user.Id),
                PortfolioAllocation = await _analyticsService.GetPortfolioAllocation(user.Id)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> InvestmentPreferences()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // In a real app, you would fetch the user's investment preferences
            // For now, we'll just return the view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvestmentPreferences(InvestmentPreferenceViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // In a real app, you would update the user's investment preferences
            // For now, we'll just redirect back to the dashboard
            return RedirectToAction(nameof(Index));
        }
    }
} 