namespace SmartPOS.Domain.Entities
{
    public class SaleItem
    {
        public int SaleItemId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Profit { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;
    }
}
