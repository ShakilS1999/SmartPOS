using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchaseController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseDto dto)
        {
            await _service.CreatePurchaseAsync(dto);
            return Ok("Purchase Completed");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllPurchasesAsync());
        }
    }
}
