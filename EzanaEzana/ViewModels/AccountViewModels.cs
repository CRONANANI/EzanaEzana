using System.ComponentModel.DataAnnotations;

namespace Ezana.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
        [Display(Name = "Public Username")]
        public string? PublicUsername { get; set; }
        
        [Display(Name = "Make Profile Public")]
        public bool IsPublicProfile { get; set; }
        
        [StringLength(160, ErrorMessage = "Bio cannot exceed 160 characters.")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }
        
        [Display(Name = "Profile Picture URL")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? ProfilePictureUrl { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
} 