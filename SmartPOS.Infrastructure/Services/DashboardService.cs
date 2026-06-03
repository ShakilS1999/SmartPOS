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
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var totalSales = await _context.Sales
                .SumAsync(s => (decimal?)s.NetTotal) ?? 0;

            var todaySales = await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => (decimal?)s.NetTotal) ?? 0;

            var thisMonthSales = await _context.Sales
                .Where(s => s.SaleDate >= firstDayOfMonth)
                .SumAsync(s => (decimal?)s.NetTotal) ?? 0;

            var thisMonthOrders = await _context.Sales
                .Where(s => s.SaleDate >= firstDayOfMonth)
                .CountAsync();

            var totalOrders = await _context.Sales.CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            var totalProfit = await _context.SaleItems
                .SumAsync(x => (decimal?)x.Profit) ?? 0;

            var todayProfit = await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SelectMany(s => s.Items)
                .SumAsync(x => (decimal?)x.Profit) ?? 0;

            return new DashboardDto
            {
                TotalSales = totalSales,
                TodaySales = todaySales,
                TotalOrders = totalOrders,
                TotalProducts = totalProducts,
                ThisMonthSales = thisMonthSales,
                ThisMonthOrders = thisMonthOrders,
                TotalProfit = totalProfit,
                TodayProfit = todayProfit
            };
        }
    }
}