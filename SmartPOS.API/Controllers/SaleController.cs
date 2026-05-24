using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _service;

        public SaleController(ISaleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllSalesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _service.GetByIdAsync(id);

            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaleDto dto)
        {
            await _service.CreateSaleAsync(dto);

            return Ok("Sale Completed");
        }
    }
}