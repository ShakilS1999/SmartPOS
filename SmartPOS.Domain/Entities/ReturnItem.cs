namespace SmartPOS.Domain.Entities
{
    public class ReturnItem
    {
        public int ReturnItemId { get; set; }
        public int ReturnId { get; set; }
        public Return Return { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}