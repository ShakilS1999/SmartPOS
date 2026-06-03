namespace SmartPOS.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalSales { get; set; }
        public decimal TodaySales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal ThisMonthSales { get; set; }
        public int ThisMonthOrders { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TodayProfit { get; set; }
    }
}