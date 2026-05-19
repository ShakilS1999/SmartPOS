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

                    // STOCK INCREASE
                    product.StockQuantity += item.Quantity;

                    // UPDATE COST PRICE
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
    }
}