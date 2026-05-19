using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;

namespace SmartPOS.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Barcode = p.Barcode,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);

            if (p == null)
                return null;

            return new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Barcode = p.Barcode,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            };
        }

        public async Task CreateAsync(ProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
                throw new Exception("Product name is required");

            if (dto.Price <= 0)
                throw new Exception("Price must be greater than 0");

            if (dto.StockQuantity < 0)
                throw new Exception("Invalid stock quantity");

            var products = await _repo.GetAllAsync();

            if (products.Any(x => x.Barcode == dto.Barcode))
                throw new Exception("Barcode already exists");

            var product = new Product
            {
                ProductName = dto.ProductName,
                Barcode = dto.Barcode,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            await _repo.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var product = await _repo.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            product.ProductName = dto.ProductName;
            product.Barcode = dto.Barcode;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            await _repo.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<ProductDto>> GetLowStockAsync()
        {
            var products = await _repo.GetAllAsync();

            return products
                .Where(p => p.StockQuantity <= 5)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Barcode = p.Barcode,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                })
                .ToList();
        }
    }
}