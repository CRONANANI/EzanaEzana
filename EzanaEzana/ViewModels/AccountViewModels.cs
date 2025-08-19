using System.ComponentModel.DataAnnotations;

namespace EzanaEzana.ViewModels
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
} 