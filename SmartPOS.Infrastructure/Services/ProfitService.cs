using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class ProfitService : IProfitService
    {
        private readonly AppDbContext _context;

        public ProfitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfitReportDto> GetProfitReportAsync()
        {
            var today = DateTime.Today;

            var totalProfit = await _context.SaleItems
                .SumAsync(x => (decimal?)x.Profit) ?? 0;

            var todayProfit = await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SelectMany(s => s.Items)
                .SumAsync(x => (decimal?)x.Profit) ?? 0;

            var totalSales = await _context.Sales.CountAsync();

            return new ProfitReportDto
            {
                TotalProfit = totalProfit,
                TodayProfit = todayProfit,
                TotalSales = totalSales
            };
        }
    }
}