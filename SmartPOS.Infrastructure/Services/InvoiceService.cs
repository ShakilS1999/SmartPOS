using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

using QuestPDF.Infrastructure;
using SmartPOS.Application.Interfaces;
using SmartPOS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefault(s => s.SaleId == saleId);

            if (sale == null)
                throw new Exception("Invoice not found");

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Smart POS Invoice")
                        .FontSize(20)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Invoice No: {sale.InvoiceNo}");
                        col.Item().Text($"Date: {sale.SaleDate}");

                        col.Item().PaddingTop(20);

                        foreach (var item in sale.Items)
                        {
                            col.Item().Text(
                                $"{item.Product.ProductName} | Qty: {item.Quantity} | Price: {item.TotalPrice}"
                            );
                        }

                        col.Item().PaddingTop(20);

                        col.Item().Text($"Grand Total: {sale.GrandTotal}")
                            .Bold();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
