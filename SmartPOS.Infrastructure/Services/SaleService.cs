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
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount,
                    Tax = dto.Tax,
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
                sale.NetTotal = total - dto.Discount + dto.Tax;

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

        public async Task<SaleDetailsDto> GetByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Include(s => s.Customer)
                .Where(s => s.SaleId == id)
                .Select(s => new SaleDetailsDto
                {
                    SaleId = s.SaleId,
                    InvoiceNo = s.InvoiceNo,
                    SaleDate = s.SaleDate,
                    GrandTotal = s.GrandTotal,
                    Discount = s.Discount,
                    Tax = s.Tax,
                    NetTotal = s.NetTotal,
                    CustomerName = s.Customer != null ? s.Customer.CustomerName : "Walk-in Customer",
                    Items = s.Items.Select(i => new SaleItemDetailsDto
                    {
                        ProductId = i.ProductId,
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
                .Include(s => s.Customer)
                .OrderByDescending(s => s.SaleDate)
                .Select(s => new SaleDetailsDto
                {
                    SaleId = s.SaleId,
                    InvoiceNo = s.InvoiceNo,
                    SaleDate = s.SaleDate,
                    GrandTotal = s.GrandTotal,
                    Discount = s.Discount,
                    Tax = s.Tax,
                    NetTotal = s.NetTotal,
                    CustomerName = s.Customer != null ? s.Customer.CustomerName : "Walk-in Customer",
                    Items = s.Items.Select(i => new SaleItemDetailsDto
                    {
                        ProductId = i.ProductId,
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