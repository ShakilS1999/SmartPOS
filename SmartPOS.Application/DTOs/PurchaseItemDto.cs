using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class PurchaseItemDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }
    }
}
