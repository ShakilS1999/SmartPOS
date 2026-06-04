namespace SmartPOS.Application.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Barcode { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal CostPrice { get; set; }

        public int StockQuantity { get; set; }
    }
}