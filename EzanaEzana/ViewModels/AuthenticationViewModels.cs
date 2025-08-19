using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
{
    // Authentication ViewModels
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "I agree to the terms and conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms and conditions")]
        public bool AgreeToTerms { get; set; }

        [Display(Name = "I agree to receive marketing communications")]
        public bool AgreeToMarketing { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reset token is required")]
        public string Token { get; set; } = string.Empty;
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
        public string? LoginProvider { get; set; }
    }

    public class TwoFactorAuthenticationViewModel
    {
        [Required(ErrorMessage = "Two-factor code is required")]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "Two-factor code must be between 6 and 7 characters")]
        [Display(Name = "Two-Factor Code")]
        public string TwoFactorCode { get; set; } = string.Empty;

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class EnableAuthenticatorViewModel
    {
        [Required(ErrorMessage = "Authenticator key is required")]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "Authenticator key must be between 6 and 7 characters")]
        [Display(Name = "Authenticator Key")]
        public string AuthenticatorKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "Verification code must be between 6 and 7 characters")]
        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; } = string.Empty;
    }
}
