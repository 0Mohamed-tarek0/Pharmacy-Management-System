using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels.Shifts
{
    public class OpenShiftViewModel
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Opening cash cannot be negative.")]
        [Display(Name = "Cash currently in the drawer")]
        public decimal OpeningCash { get; set; }
    }
}
