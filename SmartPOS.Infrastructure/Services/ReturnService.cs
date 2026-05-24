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
                var sale = await _context.Sales
                    .Include(s => s.Items)
                    .FirstOrDefaultAsync(s => s.SaleId == dto.SaleId);

                if (sale == null)
                    throw new Exception("Sale not found");

                var returnEntity = new Return
                {
                    SaleId = dto.SaleId,
                    ReturnDate = DateTime.Now,
                    Reason = dto.Reason,
                    Items = new List<ReturnItem>()
                };

                decimal refundTotal = 0;

                foreach (var item in dto.Items)
                {
                    var saleItem = sale.Items
                        .FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (saleItem == null)
                        throw new Exception($"Product not found in sale");

                    if (item.Quantity > saleItem.Quantity)
                        throw new Exception($"Return quantity exceeds sold quantity");

                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
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