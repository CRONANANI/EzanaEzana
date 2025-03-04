using Ezana.Models;
using Ezana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ezana.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PublicUsername = user.PublicUsername,
                IsPublicProfile = user.IsPublicProfile,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the username is already taken
            if (!string.IsNullOrEmpty(model.PublicUsername) && model.PublicUsername != user.PublicUsername)
            {
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PublicUsername == model.PublicUsername);
                
                if (existingUser != null)
                {
                    ModelState.AddModelError("PublicUsername", "This username is already taken. Please choose another one.");
                    return View(model);
                }
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.PublicUsername = model.PublicUsername;
            user.IsPublicProfile = model.IsPublicProfile;
            user.Bio = model.Bio;
            user.ProfilePictureUrl = model.ProfilePictureUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your password has been changed";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost, ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
} 