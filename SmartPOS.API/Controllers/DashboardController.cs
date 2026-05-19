using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            var totalProducts = _context.Products.Count();

            var totalSales = _context.Sales.Count();

            var totalRevenue = _context.SaleItems
                .Sum(x => x.TotalPrice);

            var totalProfit = _context.SaleItems
                .Sum(x => x.Profit);

            return Ok(new
            {
                totalProducts,
                totalSales,
                totalRevenue,
                totalProfit
            });
        }
    }
}