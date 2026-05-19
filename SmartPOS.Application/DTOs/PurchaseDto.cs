using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class PurchaseDto
    {
        public List<PurchaseItemDto> Items { get; set; }
    }
}
