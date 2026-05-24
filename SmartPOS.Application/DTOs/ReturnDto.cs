namespace SmartPOS.Application.DTOs
{
    public class ReturnDto
    {
        public int SaleId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public List<ReturnItemDto> Items { get; set; } = new();
    }

    public class ReturnItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ReturnDetailsDto
    {
        public int ReturnId { get; set; }
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime ReturnDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
        public List<ReturnItemDetailsDto> Items { get; set; } = new();
    }

    public class ReturnItemDetailsDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}