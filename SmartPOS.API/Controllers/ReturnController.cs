using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _service;

        public ReturnController(IReturnService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReturnDto dto)
        {
            await _service.CreateReturnAsync(dto);
            return Ok("Return Processed");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllReturnsAsync());
        }
    }
}