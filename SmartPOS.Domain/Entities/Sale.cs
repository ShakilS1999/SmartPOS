namespace SmartPOS.Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal GrandTotal { get; set; }

        public List<SaleItem> Items { get; set; }
    }
}