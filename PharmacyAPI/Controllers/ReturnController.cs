using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var returns = await _returnService.GetAllAsync();
            return Ok(returns);
        }
    }
}
