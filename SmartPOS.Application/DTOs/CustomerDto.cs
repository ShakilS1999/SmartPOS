using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }
}
