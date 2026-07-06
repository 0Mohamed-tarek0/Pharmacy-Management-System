using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels.Categories
{
    public class CreateCategoryViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
