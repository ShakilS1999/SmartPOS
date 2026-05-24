using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class SaleDto
    {
        public int? CustomerId { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
