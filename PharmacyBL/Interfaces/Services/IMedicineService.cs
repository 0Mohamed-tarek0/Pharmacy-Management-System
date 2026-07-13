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

        /// <summary>Deletes a single batch belonging to the given medicine.</summary>
        Task DeleteBatchAsync(int medicineId, int batchId);

        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();

        /// <summary>All batches, across all medicines, ordered by nearest expiry date first.</summary>
        Task<IEnumerable<BatchExpiryRowDto>> GetBatchesByExpiryAsync();
    }
}
