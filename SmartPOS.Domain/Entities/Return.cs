namespace SmartPOS.Domain.Entities
{
    public class Return
    {
        public int ReturnId { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;
        public DateTime ReturnDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
        public List<ReturnItem> Items { get; set; } = new();
    }
}