using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Users;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users/pharmacist
        [HttpPost("pharmacist")]
        public async Task<IActionResult> CreatePharmacist(CreatePharmacistDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreatePharmacistAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
