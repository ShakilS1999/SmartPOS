using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class ReturnService : IReturnService
    {
        private readonly AppDbContext _context;

        public ReturnService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateReturnAsync(ReturnDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto == null)
                    throw new Exception("Return data is required");

                if (dto.SaleId <= 0)
                    throw new Exception("Invalid sale");

                if (dto.Items == null || !dto.Items.Any())
                    throw new Exception("Return items required");

                var sale = await _context.Sales
                    .Include(s => s.Items)
                    .FirstOrDefaultAsync(s => s.SaleId == dto.SaleId);

                if (sale == null)
                    throw new Exception("Sale not found");

                var groupedItems = dto.Items
                    .GroupBy(i => i.ProductId)
                    .Select(g => new ReturnItemDto
                    {
                        ProductId = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .ToList();

                var returnEntity = new Return
                {
                    SaleId = dto.SaleId,
                    ReturnDate = DateTime.Now,
                    Reason = dto.Reason?.Trim() ?? string.Empty,
                    Items = new List<ReturnItem>()
                };

                decimal refundTotal = 0;

                foreach (var item in groupedItems)
                {
                    if (item.ProductId <= 0)
                        throw new Exception("Invalid product");

                    if (item.Quantity <= 0)
                        throw new Exception("Return quantity must be greater than 0");

                    var saleItem = sale.Items
                        .FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (saleItem == null)
                        throw new Exception("Product not found in sale");

                    var alreadyReturnedQuantity = await _context.ReturnItems
                        .Where(ri => ri.Return.SaleId == dto.SaleId && ri.ProductId == item.ProductId)
                        .SumAsync(ri => (int?)ri.Quantity) ?? 0;

                    var remainingReturnableQuantity = saleItem.Quantity - alreadyReturnedQuantity;

                    if (remainingReturnableQuantity <= 0)
                        throw new Exception("This product has already been fully returned");

                    if (item.Quantity > remainingReturnableQuantity)
                        throw new Exception($"Return quantity exceeds remaining returnable quantity. Remaining: {remainingReturnableQuantity}");

                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        throw new Exception($"Product not found: {item.ProductId}");

                    product.StockQuantity += item.Quantity;

                    var returnItem = new ReturnItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = saleItem.UnitPrice,
                        TotalPrice = saleItem.UnitPrice * item.Quantity
                    };

                    refundTotal += returnItem.TotalPrice;
                    returnEntity.Items.Add(returnItem);
                }

                returnEntity.RefundAmount = refundTotal;

                await _context.Returns.AddAsync(returnEntity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<ReturnDetailsDto>> GetAllReturnsAsync()
        {
            return await _context.Returns
                .Include(r => r.Sale)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(r => r.ReturnDate)
                .Select(r => new ReturnDetailsDto
                {
                    ReturnId = r.ReturnId,
                    SaleId = r.SaleId,
                    InvoiceNo = r.Sale.InvoiceNo,
                    ReturnDate = r.ReturnDate,
                    Reason = r.Reason,
                    RefundAmount = r.RefundAmount,
                    Items = r.Items.Select(i => new ReturnItemDetailsDto
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