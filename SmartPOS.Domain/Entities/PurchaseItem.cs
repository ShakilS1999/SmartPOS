namespace SmartPOS.Domain.Entities
{
    public class PurchaseItem
    {
        public int PurchaseItemId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = null!;
    }
}
