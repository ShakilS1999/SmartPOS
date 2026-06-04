using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProfitController : ControllerBase
    {
        private readonly IProfitService _service;

        public ProfitController(IProfitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport()
        {
            var report = await _service.GetProfitReportAsync();

            return Ok(report);
        }
    }
}
