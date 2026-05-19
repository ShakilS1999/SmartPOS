using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var today = DateTime.Today;

            var totalSales = await _context.Sales
                .SumAsync(s => (decimal?)s.GrandTotal) ?? 0;

            var todaySales = await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => (decimal?)s.GrandTotal) ?? 0;

            var totalOrders = await _context.Sales.CountAsync();

            var totalProducts = await _context.Products.CountAsync();

            return new DashboardDto
            {
                TotalSales = totalSales,
                TodaySales = todaySales,
                TotalOrders = totalOrders,
                TotalProducts = totalProducts
            };
        }
    }
}