using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;

namespace PharmacyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        // GET api/medicine
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicines = await _medicineService.GetAllAsync();
            return Ok(medicines);
        }

        // GET api/medicine/expiry-report
        [HttpGet("expiry-report")]
        public async Task<IActionResult> ExpiryReport()
        {
            var batches = await _medicineService.GetBatchesByExpiryAsync();
            return Ok(batches);
        }

        // GET api/medicine/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var medicine = await _medicineService.GetDetailsAsync(id);

            if (medicine == null)
                return NotFound();

            return Ok(medicine);
        }

        // GET api/medicine/create-options
        // MVC's Create() GET action populated ViewBag.Categories and
        // ViewBag.Types so the Razor form dropdowns had data to show.
        // An API has no view to populate, but a client still needs that
        // reference data to build its own form, so it's exposed as JSON here.
        [HttpGet("create-options")]
        public async Task<IActionResult> GetCreateOptions()
        {
            var categories = await _medicineService.GetCategoriesAsync();
            var types = Enum.GetValues(typeof(MedicineType))
                             .Cast<MedicineType>()
                             .Select(t => new { id = (int)t, name = t.ToString() });

            return Ok(new { categories, types });
        }

        // POST api/medicine
        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _medicineService.CreateAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new { message = "Medicine created successfully." });
        }

        // PUT api/medicine/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMedicineDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Route id and body id do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _medicineService.UpdateAsync(dto);

            return NoContent();
        }

        // DELETE api/medicine/5
        // MVC's Delete flow was a GET confirmation page + a POST DeleteConfirmed
        // action, because a browser needs a page to show "are you sure?" on.
        // An API just exposes a single DELETE endpoint; the client's UI is
        // responsible for showing its own confirmation before calling it.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _medicineService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE api/medicine/5/batch/3
        [HttpDelete("{medicineId}/batch/{batchId}")]
        public async Task<IActionResult> DeleteBatch(int medicineId, int batchId)
        {
            try
            {
                await _medicineService.DeleteBatchAsync(medicineId, batchId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
