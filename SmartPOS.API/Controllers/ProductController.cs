using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            if (id != dto.ProductId)
                return BadRequest();

            await _service.UpdateAsync(dto);

            return Ok("Product Updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                message = "Product Deleted"
            });
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> GetLowStock()
        {
            var products = await _service.GetLowStockAsync();

            return Ok(products);
        }
    }
}
