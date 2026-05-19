using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartPOS.Application.DTOs
{
    public class SaleDetailsDto
    {
        public string InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal GrandTotal { get; set; }

        public List<SaleItemDetailsDto> Items { get; set; }
    }
}
