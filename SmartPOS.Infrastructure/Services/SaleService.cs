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
                if (dto == null)
                    throw new Exception("Sale data is required");

                if (dto.Items == null || !dto.Items.Any())
                    throw new Exception("Sale items required");

                if (dto.Discount < 0)
                    throw new Exception("Discount cannot be negative");

                if (dto.Tax < 0)
                    throw new Exception("Tax cannot be negative");

                if (dto.PaidAmount < 0)
                    throw new Exception("Paid amount cannot be negative");

                if (dto.CustomerId.HasValue)
                {
                    var customerExists = await _context.Customers
                        .AnyAsync(c => c.CustomerId == dto.CustomerId.Value);

                    if (!customerExists)
                        throw new Exception("Customer not found");
                }

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
                    if (item.ProductId <= 0)
                        throw new Exception("Invalid product");

                    if (item.Quantity <= 0)
                        throw new Exception("Quantity must be greater than 0");

                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        throw new Exception($"Product not found: {item.ProductId}");

                    if (product.Price <= 0)
                        throw new Exception($"{product.ProductName} price is invalid");

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

                if (dto.Discount > total)
                    throw new Exception("Discount cannot be greater than grand total");

                sale.GrandTotal = total;
                sale.NetTotal = total - dto.Discount + dto.Tax;

                if (sale.NetTotal < 0)
                    throw new Exception("Net total cannot be negative");

                if (dto.PaidAmount > sale.NetTotal)
                    throw new Exception("Paid amount cannot be greater than net total");

                sale.PaidAmount = dto.PaidAmount;
                sale.DueAmount = sale.NetTotal - dto.PaidAmount;

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

        public async Task<SaleDetailsDto?> GetByIdAsync(int id)
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
                    PaidAmount = s.PaidAmount,
                    DueAmount = s.DueAmount,
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
                    PaidAmount = s.PaidAmount,
                    DueAmount = s.DueAmount,
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
