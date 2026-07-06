using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels.Suppliers
{
    public class UpdateSupplierViewModel
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
