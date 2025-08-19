using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic; // Added for List
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(
            UserManager<ApplicationUser> userManager,
            IPortfolioService portfolioService)
        {
            _userManager = userManager;
            _portfolioService = portfolioService;
        }

        // GET: /Portfolio
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolios = await _portfolioService.GetUserPortfoliosAsync(user.Id);
                var defaultPortfolio = await _portfolioService.GetDefaultPortfolioAsync(user.Id);
                
                var viewModel = new PortfolioIndexViewModel
                {
                    Portfolios = portfolios,
                    DefaultPortfolio = defaultPortfolio
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id, user.Id);
                if (portfolio == null)
                {
                    return NotFound();
                }

                var summary = await _portfolioService.GetPortfolioSummaryAsync(id, user.Id);
                var topHoldings = await _portfolioService.GetTopHoldingsAsync(id, user.Id, 5);

                var viewModel = new PortfolioDetailsViewModel
                {
                    Portfolio = portfolio,
                    Summary = summary,
                    TopHoldings = topHoldings
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/Create
        public IActionResult Create()
        {
            return View(new CreatePortfolioViewModel());
        }

        // POST: /Portfolio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePortfolioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.CreatePortfolioAsync(model, user.Id);
                return RedirectToAction(nameof(Details), new { id = portfolio.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: /Portfolio/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id, user.Id);
                if (portfolio == null)
                {
                    return NotFound();
                }

                var viewModel = new UpdatePortfolioViewModel
                {
                    Name = portfolio.Name,
                    Description = portfolio.Description,
                    IsPublic = portfolio.IsPublic
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: /Portfolio/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePortfolioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.UpdatePortfolioAsync(id, model, user.Id);
                return RedirectToAction(nameof(Details), new { id = portfolio.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // POST: /Portfolio/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var success = await _portfolioService.DeletePortfolioAsync(id, user.Id);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("Failed to delete portfolio");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /Portfolio/Performance/{id}
        public async Task<IActionResult> Performance(int id, int days = 365)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var performance = await _portfolioService.GetPortfolioPerformanceAsync(id, user.Id, days);
                return View(performance);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/Allocation/{id}
        public async Task<IActionResult> Allocation(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var allocation = await _portfolioService.GetPortfolioAllocationAsync(id, user.Id);
                return View(allocation);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/Transactions/{id}
        public async Task<IActionResult> Transactions(int id, int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var transactions = await _portfolioService.GetPortfolioTransactionsAsync(id, user.Id, page, 50);
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id, user.Id);

                var viewModel = new TransactionsViewModel
                {
                    Portfolio = portfolio,
                    Transactions = transactions,
                    CurrentPage = page
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/AddHolding/{id}
        public async Task<IActionResult> AddHolding(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id, user.Id);
                if (portfolio == null)
                {
                    return NotFound();
                }

                ViewBag.PortfolioId = id;
                ViewBag.PortfolioName = portfolio.Name;
                return View(new AddHoldingViewModel());
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: /Portfolio/AddHolding/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHolding(int id, AddHoldingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PortfolioId = id;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var holding = await _portfolioService.AddHoldingAsync(id, model, user.Id);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.PortfolioId = id;
                return View(model);
            }
        }

        // GET: /Portfolio/AddTransaction/{id}
        public async Task<IActionResult> AddTransaction(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(id, user.Id);
                if (portfolio == null)
                {
                    return NotFound();
                }

                ViewBag.PortfolioId = id;
                ViewBag.PortfolioName = portfolio.Name;
                return View(new AddTransactionViewModel());
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: /Portfolio/AddTransaction/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTransaction(int id, AddTransactionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PortfolioId = id;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var transaction = await _portfolioService.AddTransactionAsync(id, model, user.Id);
                return RedirectToAction(nameof(Transactions), new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.PortfolioId = id;
                return View(model);
            }
        }

        // GET: /Portfolio/Watchlists
        public async Task<IActionResult> Watchlists()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var watchlists = await _portfolioService.GetUserWatchlistsAsync(user.Id);
                return View(watchlists);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/Watchlist/{id}
        public async Task<IActionResult> Watchlist(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var watchlist = await _portfolioService.GetWatchlistByIdAsync(id, user.Id);
                if (watchlist == null)
                {
                    return NotFound();
                }

                var items = await _portfolioService.GetWatchlistItemsAsync(id, user.Id);

                var viewModel = new WatchlistDetailsViewModel
                {
                    Watchlist = watchlist,
                    Items = items
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Portfolio/CreateWatchlist
        public IActionResult CreateWatchlist()
        {
            return View(new CreateWatchlistViewModel());
        }

        // POST: /Portfolio/CreateWatchlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWatchlist(CreateWatchlistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var watchlist = await _portfolioService.CreateWatchlistAsync(model, user.Id);
                return RedirectToAction(nameof(Watchlist), new { id = watchlist.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // POST: /Portfolio/UpdatePrices/{id}
        [HttpPost]
        public async Task<IActionResult> UpdatePrices(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                await _portfolioService.UpdatePortfolioPricesAsync(id, user.Id);
                return Json(new { success = true, message = "Portfolio prices updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Portfolio/API/Summary/{id}
        [HttpGet]
        [Route("api/portfolio/{id}/summary")]
        public async Task<IActionResult> GetPortfolioSummary(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var summary = await _portfolioService.GetPortfolioSummaryAsync(id, user.Id);
                return Json(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Portfolio/API/Performance/{id}
        [HttpGet]
        [Route("api/portfolio/{id}/performance")]
        public async Task<IActionResult> GetPortfolioPerformance(int id, int days = 365)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var performance = await _portfolioService.GetPortfolioPerformanceAsync(id, user.Id, days);
                return Json(performance);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class PortfolioIndexViewModel
    {
        public List<Portfolio> Portfolios { get; set; }
        public Portfolio DefaultPortfolio { get; set; }
    }

    public class PortfolioDetailsViewModel
    {
        public Portfolio Portfolio { get; set; }
        public PortfolioSummaryViewModel Summary { get; set; }
        public List<PortfolioHolding> TopHoldings { get; set; }
    }

    public class TransactionsViewModel
    {
        public Portfolio Portfolio { get; set; }
        public List<PortfolioTransaction> Transactions { get; set; }
        public int CurrentPage { get; set; }
    }

    public class WatchlistDetailsViewModel
    {
        public Watchlist Watchlist { get; set; }
        public List<WatchlistItem> Items { get; set; }
    }
}
