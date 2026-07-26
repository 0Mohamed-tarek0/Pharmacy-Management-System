using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Shifts;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService) => _shiftService = shiftService;

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _shiftService.GetDashboardAsync(CurrentUserId(), User.IsInRole("Admin"));
            return Ok(dashboard);
        }

        // POST api/shift/open
        // MVC's GET Open() pre-checked for an existing open shift before even
        // showing the form. The API folds that check into the POST itself,
        // since there's no separate "show form" step for a JSON endpoint.
        [HttpPost("open")]
        public async Task<IActionResult> Open(OpenShiftDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ApplicationUserId = CurrentUserId();

            var opened = await _shiftService.OpenAsync(dto);
            if (!opened)
                return Conflict(new { message = "A shift could not be opened. You may already have one open." });

            return Ok(new { message = "Shift opened successfully." });
        }

        [HttpPost("close")]
        public async Task<IActionResult> Close(CloseShiftDto dto)
        {
            var shift = await _shiftService.GetByIdAsync(dto.Id, CurrentUserId());
            if (shift == null || !shift.IsOpen)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ApplicationUserId = CurrentUserId();

            var closed = await _shiftService.CloseAsync(dto);
            if (closed == null)
                return BadRequest(new { message = "This shift could not be closed. Please try again." });

            return Ok(closed);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id, CurrentUserId(), User.IsInRole("Admin"));
            return shift == null ? NotFound() : Ok(shift);
        }

        private string CurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
