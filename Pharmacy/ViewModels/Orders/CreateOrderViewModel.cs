using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyBL.DTOs.Orders;

namespace Pharmacy.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        public CreateOrderDto Order { get; set; } = new();

        public IEnumerable<SelectListItem> Suppliers { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Medicines { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
