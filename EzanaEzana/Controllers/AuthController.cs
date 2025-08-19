using EzanaEzana.Models;
using EzanaEzana.Services;
using EzanaEzana.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EzanaEzana.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Update last login
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    // Generate JWT token for API access
                    var token = _jwtService.GenerateToken(user);
                    Response.Cookies.Append("auth_token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                return RedirectToAction("Index", "Dashboard");
            }
            
            if (result.RequiresTwoFactor)
            {
                // TODO: Implement two-factor authentication
                ModelState.AddModelError(string.Empty, "Two-factor authentication is required.");
                return View(model);
            }
            
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked. Please try again later.");
                return View(model);
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if username is already taken
            if (!string.IsNullOrEmpty(model.PublicUsername))
            {
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PublicUsername == model.PublicUsername);
                
                if (existingUser != null)
                {
                    ModelState.AddModelError("PublicUsername", "This username is already taken. Please choose another one.");
                    return View(model);
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PublicUsername = model.PublicUsername,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                // Generate JWT token
                var token = _jwtService.GenerateToken(user);
                Response.Cookies.Append("auth_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                TempData["StatusMessage"] = "Account created successfully! Welcome to Ezana.";
                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            // Remove JWT token cookie
            Response.Cookies.Delete("auth_token");
            
            TempData["StatusMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Auth/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // TODO: Implement password reset email functionality
            // For now, just show confirmation
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        // GET: /Auth/ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Auth/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string? code = null)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Auth/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: /Auth/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // API endpoint for external authentication (React frontend)
        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> ApiLogin([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    var token = _jwtService.GenerateToken(user);
                    return Ok(new { 
                        success = true, 
                        token = token,
                        user = new {
                            id = user.Id,
                            email = user.Email,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            publicUsername = user.PublicUsername
                        }
                    });
                }
            }

            return Unauthorized(new { success = false, message = "Invalid login attempt" });
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> ApiRegister([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid model state", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            if (!string.IsNullOrEmpty(model.PublicUsername))
            {
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PublicUsername == model.PublicUsername);
                
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Username is already taken" });
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PublicUsername = model.PublicUsername,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                var token = _jwtService.GenerateToken(user);
                return Ok(new { 
                    success = true, 
                    token = token,
                    user = new {
                        id = user.Id,
                        email = user.Email,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        publicUsername = user.PublicUsername
                    }
                });
            }

            return BadRequest(new { 
                success = false, 
                message = "Registration failed", 
                errors = result.Errors.Select(e => e.Description) 
            });
        }
    }
}
