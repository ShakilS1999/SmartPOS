namespace SmartPOS.Domain.Entities
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal GrandTotal { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public List<PurchaseItem> Items { get; set; } = new();
    }
}