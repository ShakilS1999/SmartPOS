namespace SmartPOS.Application.DTOs
{
    public class SaleDetailsDto
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal NetTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string? CustomerName { get; set; }
        public List<SaleItemDetailsDto> Items { get; set; } = new();
    }
}