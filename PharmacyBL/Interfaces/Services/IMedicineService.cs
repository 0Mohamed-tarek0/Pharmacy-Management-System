using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.DTOs.Medicines;

namespace PharmacyBL.Interfaces.Services
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDto>> GetAllAsync();

        Task<UpdateMedicineDto?> GetByIdAsync(int id);

        /// <summary>Medicine details screen: medicine + all its batches + units.</summary>
        Task<MedicineDetailsDto?> GetDetailsAsync(int id);

        Task CreateAsync(CreateMedicineDto dto);

        Task UpdateAsync(UpdateMedicineDto dto);

        Task DeleteAsync(int id);

        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}
