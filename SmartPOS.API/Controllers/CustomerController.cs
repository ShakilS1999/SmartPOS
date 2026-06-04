using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
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
            var customer = await _service.GetByIdAsync(id);

            if (customer == null)
                return NotFound("Customer not found");

            return Ok(customer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Cashier")]
        public async Task<IActionResult> Create(CustomerDto dto)
        {
            await _service.CreateAsync(dto);

            return Ok("Customer Created");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, CustomerDto dto)
        {
            if (id != dto.CustomerId)
                return BadRequest();

            await _service.UpdateAsync(dto);

            return Ok("Customer Updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok("Customer Deleted");
        }
    }
}
