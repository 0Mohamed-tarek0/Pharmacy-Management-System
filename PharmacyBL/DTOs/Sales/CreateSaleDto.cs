using System.Collections.Generic;

namespace PharmacyBL.DTOs.Sales
{
    public class CreateSaleDto
    {
        public string ApplicationUserId { get; set; } = string.Empty;

        public List<SaleItemInputDto> Items { get; set; } = new();
    }
}
