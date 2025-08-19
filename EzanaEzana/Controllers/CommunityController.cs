using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommunityService _communityService;

        public CommunityController(
            UserManager<ApplicationUser> userManager,
            ICommunityService communityService)
        {
            _userManager = userManager;
            _communityService = communityService;
        }

        // GET: /Community/Threads
        public async Task<IActionResult> Threads(int page = 1, string category = null)
        {
            try
            {
                var threads = await _communityService.GetThreadsAsync(page, 20, category);
                var viewModel = new ThreadsViewModel
                {
                    Threads = threads,
                    CurrentPage = page,
                    Category = category
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Community/Thread/{id}
        public async Task<IActionResult> Thread(int id)
        {
            try
            {
                var thread = await _communityService.GetThreadByIdAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                var comments = await _communityService.GetThreadCommentsAsync(id);
                var statistics = await _communityService.GetThreadStatisticsAsync(id);

                var viewModel = new ThreadDetailViewModel
                {
                    Thread = thread,
                    Comments = comments,
                    Statistics = statistics
                };

                // Record view
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _communityService.ViewThreadAsync(id, user.Id);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Community/Create
        public IActionResult Create()
        {
            return View(new CreateThreadViewModel());
        }

        // POST: /Community/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateThreadViewModel model)
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
                var thread = await _communityService.CreateThreadAsync(model, user.Id);
                return RedirectToAction(nameof(Thread), new { id = thread.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // GET: /Community/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var thread = await _communityService.GetThreadByIdAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                if (thread.AuthorId != user.Id)
                {
                    return Forbid();
                }

                var viewModel = new UpdateThreadViewModel
                {
                    Title = thread.Title,
                    Content = thread.Content,
                    Category = thread.Category,
                    Tags = thread.Tags
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: /Community/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateThreadViewModel model)
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
                var thread = await _communityService.UpdateThreadAsync(id, model, user.Id);
                return RedirectToAction(nameof(Thread), new { id = thread.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // POST: /Community/Delete/{id}
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
                var success = await _communityService.DeleteThreadAsync(id, user.Id);
                if (success)
                {
                    return RedirectToAction(nameof(Threads));
                }
                else
                {
                    return BadRequest("Failed to delete thread");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: /Community/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(AddCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var comment = await _communityService.AddCommentAsync(model, user.Id);
                return Json(new { success = true, comment = comment });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Community/LikeThread/{id}
        [HttpPost]
        public async Task<IActionResult> LikeThread(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var success = await _communityService.LikeThreadAsync(id, user.Id);
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Community/UnlikeThread/{id}
        [HttpPost]
        public async Task<IActionResult> UnlikeThread(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var success = await _communityService.UnlikeThreadAsync(id, user.Id);
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Community/LikeComment/{id}
        [HttpPost]
        public async Task<IActionResult> LikeComment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var success = await _communityService.LikeCommentAsync(id, user.Id);
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: /Community/UnlikeComment/{id}
        [HttpPost]
        public async Task<IActionResult> UnlikeComment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var success = await _communityService.UnlikeCommentAsync(id, user.Id);
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Community/Search
        public async Task<IActionResult> Search(string q, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction(nameof(Threads));
            }

            try
            {
                var threads = await _communityService.SearchThreadsAsync(q, page, 20);
                var viewModel = new SearchResultsViewModel
                {
                    Threads = threads,
                    SearchTerm = q,
                    CurrentPage = page
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Community/User/{userId}
        public async Task<IActionResult> UserThreads(string userId, int page = 1)
        {
            try
            {
                var threads = await _communityService.GetThreadsByUserAsync(userId, page, 20);
                var stats = await _communityService.GetUserCommunityStatsAsync(userId);

                var viewModel = new UserThreadsViewModel
                {
                    Threads = threads,
                    Statistics = stats,
                    CurrentPage = page
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: /Community/API/Threads
        [HttpGet]
        [Route("api/community/threads")]
        public async Task<IActionResult> GetThreads(int page = 1, int pageSize = 20, string category = null)
        {
            try
            {
                var threads = await _communityService.GetThreadsAsync(page, pageSize, category);
                return Json(threads);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Community/API/Thread/{id}
        [HttpGet]
        [Route("api/community/thread/{id}")]
        public async Task<IActionResult> GetThread(int id)
        {
            try
            {
                var thread = await _communityService.GetThreadByIdAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                return Json(thread);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Community/API/Thread/{id}/Comments
        [HttpGet]
        [Route("api/community/thread/{id}/comments")]
        public async Task<IActionResult> GetThreadComments(int id, int page = 1, int pageSize = 50)
        {
            try
            {
                var comments = await _communityService.GetThreadCommentsAsync(id, page, pageSize);
                return Json(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: /Community/API/Statistics/{id}
        [HttpGet]
        [Route("api/community/thread/{id}/statistics")]
        public async Task<IActionResult> GetThreadStatistics(int id)
        {
            try
            {
                var statistics = await _communityService.GetThreadStatisticsAsync(id);
                return Json(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class ThreadsViewModel
    {
        public List<CommunityThread> Threads { get; set; }
        public int CurrentPage { get; set; }
        public string Category { get; set; }
    }

    public class ThreadDetailViewModel
    {
        public CommunityThread Thread { get; set; }
        public List<ThreadComment> Comments { get; set; }
        public ThreadStatisticsViewModel Statistics { get; set; }
    }

    public class SearchResultsViewModel
    {
        public List<CommunityThread> Threads { get; set; }
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; }
    }

    public class UserThreadsViewModel
    {
        public List<CommunityThread> Threads { get; set; }
        public UserCommunityStatsViewModel Statistics { get; set; }
        public int CurrentPage { get; set; }
    }
}
