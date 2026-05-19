using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.DTOs
{
    public class ProfitReportDto
    {
        public decimal TotalProfit { get; set; }

        public decimal TodayProfit { get; set; }

        public int TotalSales { get; set; }
    }
}
