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
    public class SocialController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISocialService _socialService;

        public SocialController(
            UserManager<ApplicationUser> userManager,
            ISocialService socialService)
        {
            _userManager = userManager;
            _socialService = socialService;
        }

        // User search
        public async Task<IActionResult> Search(string searchTerm)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var viewModel = new UserSearchViewModel
            {
                SearchTerm = searchTerm
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                viewModel.Results = await _socialService.SearchUsers(searchTerm, user.Id);
            }

            return View(viewModel);
        }

        // User profile
        public async Task<IActionResult> Profile(string username)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            try
            {
                var viewModel = await _socialService.GetUserProfile(username, currentUser.Id);
                return View(viewModel);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // Friends management
        public async Task<IActionResult> Friends()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var viewModel = await _socialService.GetUserFriends(user.Id);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.SendFriendRequest(user.Id, userId);
            return RedirectToAction(nameof(Friends));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.AcceptFriendRequest(requestId, user.Id);
            return RedirectToAction(nameof(Friends));
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(int requestId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.RejectFriendRequest(requestId, user.Id);
            return RedirectToAction(nameof(Friends));
        }

        [HttpPost]
        public async Task<IActionResult> CancelFriendRequest(int requestId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.CancelFriendRequest(requestId, user.Id);
            return RedirectToAction(nameof(Friends));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.RemoveFriend(user.Id, friendId);
            return RedirectToAction(nameof(Friends));
        }

        // Messaging
        public async Task<IActionResult> Messages(string userId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var viewModel = await _socialService.GetUserConversations(user.Id);

            if (!string.IsNullOrEmpty(userId))
            {
                viewModel.CurrentConversation = await _socialService.GetConversation(user.Id, userId);
                viewModel.NewMessage = new SendMessageViewModel { RecipientId = userId };
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await _socialService.GetUserConversations(user.Id);
                viewModel.CurrentConversation = await _socialService.GetConversation(user.Id, model.RecipientId);
                viewModel.NewMessage = model;
                return View("Messages", viewModel);
            }

            try
            {
                await _socialService.SendMessage(user.Id, model.RecipientId, model.Content);
                return RedirectToAction(nameof(Messages), new { userId = model.RecipientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var viewModel = await _socialService.GetUserConversations(user.Id);
                viewModel.CurrentConversation = await _socialService.GetConversation(user.Id, model.RecipientId);
                viewModel.NewMessage = model;
                return View("Messages", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int messageId, string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _socialService.MarkMessageAsRead(messageId, user.Id);
            return RedirectToAction(nameof(Messages), new { userId });
        }
    }
} 