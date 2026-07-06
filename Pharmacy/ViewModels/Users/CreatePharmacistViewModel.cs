using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels.Users
{
    public class CreatePharmacistViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
    }
}
