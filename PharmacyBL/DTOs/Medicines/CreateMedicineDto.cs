using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Enums;

namespace PharmacyBL.DTOs.Medicines
{
    public class CreateMedicineDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public MedicineType Type { get; set; }

        public string? Description { get; set; }

        [Range(0, 100000)]
        public int MinimumStock { get; set; }

        public string? Barcode { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public int CategoryId { get; set; }

        /// <summary>Name of the smallest sellable unit, e.g. "Strip", "Ampoule", "Tube", "Bottle".</summary>
        [Required]
        public string BaseUnitName { get; set; } = "Unit";
    }
}
