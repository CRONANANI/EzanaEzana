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
    public class GRPVController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGRPVService _grpvService;

        public GRPVController(
            UserManager<ApplicationUser> userManager,
            IGRPVService grpvService)
        {
            _userManager = userManager;
            _grpvService = grpvService;
        }

        public async Task<IActionResult> Index(string searchQuery = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var viewModel = new GRPVViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName ?? "User",
                UserGRPVModels = await _grpvService.GetUserGRPVModels(user.Id),
                AvailableStocks = await _grpvService.SearchStocks(searchQuery),
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var model = await _grpvService.GetGRPVModelById(id, user.Id);
            if (model == null)
            {
                return NotFound();
            }

            var viewModel = new GRPVDetailViewModel
            {
                Model = model
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(string stockSymbol)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrEmpty(stockSymbol))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var model = await _grpvService.CalculateGRPVScores(stockSymbol, user.Id);
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error calculating GRPV scores: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _grpvService.DeleteGRPVModel(id, user.Id);
            return RedirectToAction(nameof(Index));
        }
    }
} 