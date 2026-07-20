using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PharmacyBL.DTOs.Shifts;

namespace Pharmacy.ViewModels.Shifts
{
    public class CloseShiftViewModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public ShiftDto Shift { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Actual cash cannot be negative.")]
        [Display(Name = "Cash counted in the drawer")]
        public decimal ActualCash { get; set; }
    }
}
