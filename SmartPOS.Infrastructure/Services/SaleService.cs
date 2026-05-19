using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class SaleService : ISaleService
    {
        private readonly AppDbContext _context;

        public SaleService(AppDbContext context)
        {
            _context = context;
        }

        //Create Sale (Invoice)
        public async Task CreateSaleAsync(SaleDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto == null || dto.Items == null || !dto.Items.Any())
                    throw new Exception("Sale items required");

                var sale = new Sale
                {
                    InvoiceNo = "INV-" + DateTime.Now.Ticks,
                    SaleDate = DateTime.Now,
                    Items = new List<SaleItem>()
                };

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        throw new Exception($"Product not found: {item.ProductId}");

                    if (item.Quantity <= 0)
                        throw new Exception("Quantity must be greater than 0");

                    if (product.StockQuantity < item.Quantity)
                        throw new Exception($"{product.ProductName} stock not available");

                    product.StockQuantity -= item.Quantity;

                    var saleItem = new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price * item.Quantity,
                        Profit = (product.Price - product.CostPrice) * item.Quantity
                    };

                    total += saleItem.TotalPrice;
                    sale.Items.Add(saleItem);
                }

                sale.GrandTotal = total;

                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Get Sale by Id (Invoice Details)
        public async Task<SaleDetailsDto> GetByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Where(s => s.SaleId == id)
                .Select(s => new SaleDetailsDto
                {
                    InvoiceNo = s.InvoiceNo,
                    SaleDate = s.SaleDate,
                    GrandTotal = s.GrandTotal,
                    Items = s.Items.Select(i => new SaleItemDetailsDto
                    {
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return sale;
        }
        public async Task<List<SaleDetailsDto>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(s => s.SaleDate)
                .Select(s => new SaleDetailsDto
                {
                    InvoiceNo = s.InvoiceNo,
                    SaleDate = s.SaleDate,
                    GrandTotal = s.GrandTotal,

                    Items = s.Items.Select(i => new SaleItemDetailsDto
                    {
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}