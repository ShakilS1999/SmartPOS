using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SmartPOS.Application.Interfaces;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public byte[] GenerateInvoicePdf(int saleId)
        {
            var sale = _context.Sales
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .Include(s => s.Customer)
                .FirstOrDefault(s => s.SaleId == saleId);

            if (sale == null)
                throw new Exception("Invoice not found");

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Column(header =>
                    {
                        header.Item().Text("Smart POS")
                            .FontSize(24)
                            .Bold()
                            .AlignCenter();

                        header.Item().Text("Your Trusted POS System")
                            .FontSize(12)
                            .AlignCenter();

                        header.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().Column(col =>
                    {
                        // Invoice Info
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text($"Invoice No: {sale.InvoiceNo}").Bold();
                                c.Item().Text($"Date: {sale.SaleDate:dd/MM/yyyy HH:mm}");
                                c.Item().Text($"Customer: {(sale.Customer != null ? sale.Customer.CustomerName : "Walk-in Customer")}");
                            });
                        });

                        col.Item().PaddingTop(10).LineHorizontal(1);

                        // Items Table Header
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem(3).Text("Product").Bold();
                            row.RelativeItem(1).Text("Qty").Bold().AlignCenter();
                            row.RelativeItem(2).Text("Unit Price").Bold().AlignRight();
                            row.RelativeItem(2).Text("Total").Bold().AlignRight();
                        });

                        col.Item().LineHorizontal(0.5f);

                        // Items
                        foreach (var item in sale.Items)
                        {
                            col.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem(3).Text(item.Product.ProductName);
                                row.RelativeItem(1).Text(item.Quantity.ToString()).AlignCenter();
                                row.RelativeItem(2).Text($"৳{item.UnitPrice:0.00}").AlignRight();
                                row.RelativeItem(2).Text($"৳{item.TotalPrice:0.00}").AlignRight();
                            });
                        }

                        col.Item().PaddingTop(10).LineHorizontal(1);

                        // Summary
                        col.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem();
                            row.RelativeItem(2).Column(c =>
                            {
                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Subtotal:");
                                    r.RelativeItem().Text($"৳{sale.GrandTotal:0.00}").AlignRight();
                                });

                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Discount:");
                                    r.RelativeItem().Text($"- ৳{sale.Discount:0.00}").AlignRight();
                                });

                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Tax:");
                                    r.RelativeItem().Text($"+ ৳{sale.Tax:0.00}").AlignRight();
                                });

                                c.Item().LineHorizontal(0.5f);

                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Net Total:").Bold().FontSize(14);
                                    r.RelativeItem().Text($"৳{sale.NetTotal:0.00}").Bold().FontSize(14).AlignRight();
                                });
                            });
                        });

                        col.Item().PaddingTop(20).LineHorizontal(1);

                        // Footer
                        col.Item().PaddingTop(10).Text("Thank You for Shopping!")
                            .AlignCenter()
                            .FontSize(14);

                        col.Item().Text("Please come again")
                            .AlignCenter()
                            .FontSize(12);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}