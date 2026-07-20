using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class ReturnController : Controller
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        public async Task<IActionResult> Index()
        {
            var returns = await _returnService.GetAllAsync();

            return View(returns);
        }
    }
}
