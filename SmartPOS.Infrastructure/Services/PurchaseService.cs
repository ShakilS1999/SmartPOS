using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePurchaseAsync(PurchaseDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (dto == null || dto.Items == null || !dto.Items.Any())
                    throw new Exception("Purchase items required");

                var purchase = new Purchase
                {
                    InvoiceNo = "PUR-" + DateTime.Now.Ticks,
                    PurchaseDate = DateTime.Now,
                    SupplierId = dto.SupplierId,
                    Items = new List<PurchaseItem>()
                };

                decimal total = 0;

                foreach (var item in dto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        throw new Exception($"Product not found: {item.ProductId}");

                    if (item.Quantity <= 0)
                        throw new Exception("Quantity must be greater than 0");

                    if (item.CostPrice <= 0)
                        throw new Exception("Invalid cost price");

                    product.StockQuantity += item.Quantity;
                    product.CostPrice = item.CostPrice;

                    var purchaseItem = new PurchaseItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CostPrice = item.CostPrice,
                        TotalPrice = item.Quantity * item.CostPrice
                    };

                    total += purchaseItem.TotalPrice;
                    purchase.Items.Add(purchaseItem);
                }

                purchase.GrandTotal = total;

                await _context.Purchases.AddAsync(purchase);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<PurchaseDetailsDto>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.PurchaseDate)
                .Select(p => new PurchaseDetailsDto
                {
                    PurchaseId = p.PurchaseId,
                    InvoiceNo = p.InvoiceNo,
                    PurchaseDate = p.PurchaseDate,
                    GrandTotal = p.GrandTotal,
                    SupplierName = p.Supplier != null ? p.Supplier.SupplierName : "Unknown",
                    Items = p.Items.Select(i => new PurchaseItemDetailsDto
                    {
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        CostPrice = i.CostPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}