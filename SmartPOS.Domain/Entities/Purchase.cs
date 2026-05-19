using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Domain.Entities
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal GrandTotal { get; set; }

        public List<PurchaseItem> Items { get; set; }
    }
}
