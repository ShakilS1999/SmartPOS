namespace SmartPOS.Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetTotal { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public List<SaleItem> Items { get; set; } = new();
    }
}