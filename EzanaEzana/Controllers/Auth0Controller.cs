using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EzanaEzana.Models;
using EzanaEzana.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Auth0.AspNetCore.Authentication;

namespace EzanaEzana.Controllers
{
    public class Auth0Controller : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public Auth0Controller(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

            [HttpGet]
    public async Task Login(string returnUrl = "/")
    {
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            // Indicate here where Auth0 should redirect the user after a login.
            // Note that the resulting absolute Uri must be added to the
            // **Allowed Callback URLs** settings for the app.
            .WithRedirectUri(returnUrl)
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            try
            {
                // Get the user information from Auth0
                var result = await HttpContext.AuthenticateAsync(Auth0Constants.AuthenticationScheme);
                
                if (result?.Succeeded == true)
                {
                    var user = result.Principal;
                    var email = user.FindFirst(ClaimTypes.Email)?.Value;
                    var name = user.FindFirst(ClaimTypes.Name)?.Value;
                    var sub = user.FindFirst("sub")?.Value; // Auth0 user ID

                    if (!string.IsNullOrEmpty(email))
                    {
                        // Check if user exists in our database
                        var existingUser = await _userManager.FindByEmailAsync(email);
                        
                        if (existingUser == null)
                        {
                            // Create new user
                            var newUser = new ApplicationUser
                            {
                                UserName = email,
                                Email = email,
                                EmailConfirmed = true,
                                FirstName = name?.Split(' ').FirstOrDefault() ?? "",
                                LastName = name?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                                PublicUsername = email.Split('@')[0],
                                Auth0Id = sub // Store Auth0 ID for future reference
                            };

                            var createResult = await _userManager.CreateAsync(newUser);
                            if (createResult.Succeeded)
                            {
                                existingUser = newUser;
                            }
                        }
                        else if (!string.IsNullOrEmpty(sub))
                        {
                            // Update existing user's Auth0 ID if not set
                            if (string.IsNullOrEmpty(existingUser.Auth0Id))
                            {
                                existingUser.Auth0Id = sub;
                                await _userManager.UpdateAsync(existingUser);
                            }
                        }

                        // Sign in the user
                        await _signInManager.SignInAsync(existingUser, isPersistent: true);
                        
                        // Redirect to dashboard or return URL
                        return RedirectToAction("Index", "Dashboard");
                    }
                }

                // If we get here, something went wrong
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                // Log the exception
                return RedirectToAction("Login", "Auth");
            }
        }

            [HttpGet]
    [Authorize]
    public async Task Logout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            // Indicate here where Auth0 should redirect the user after a logout.
            // Note that the resulting absolute Uri must be added to the
            // **Allowed Logout URLs** settings for the app.
            .WithRedirectUri(Url.Action("Index", "Home"))
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

            [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        return View(new
        {
            Name = User.Identity?.Name,
            EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
        });
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
    }
}
