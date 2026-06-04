using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Supplier Created");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SupplierDto dto)
        {
            if (id != dto.SupplierId)
                return BadRequest();

            await _service.UpdateAsync(dto);
            return Ok("Supplier Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Supplier Deleted");
        }
    }
}
