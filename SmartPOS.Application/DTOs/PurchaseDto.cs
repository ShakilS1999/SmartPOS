namespace SmartPOS.Application.DTOs
{
    public class PurchaseDto
    {
        public int? SupplierId { get; set; }
        public List<PurchaseItemDto> Items { get; set; } = new();
    }

    public class PurchaseDetailsDto
    {
        public int PurchaseId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal GrandTotal { get; set; }
        public string? SupplierName { get; set; }
        public List<PurchaseItemDetailsDto> Items { get; set; } = new();
    }

    public class PurchaseItemDetailsDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}