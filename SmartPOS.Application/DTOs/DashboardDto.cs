using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalSales { get; set; }
        public decimal TodaySales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
    }
}
