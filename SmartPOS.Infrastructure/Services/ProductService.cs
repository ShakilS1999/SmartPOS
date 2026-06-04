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
                CostPrice = p.CostPrice,
                StockQuantity = p.StockQuantity
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Barcode = product.Barcode,
                Price = product.Price,
                CostPrice = product.CostPrice,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task CreateAsync(ProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
                throw new Exception("Product name is required");

            if (dto.Price <= 0)
                throw new Exception("Price must be greater than 0");

            if (dto.CostPrice < 0)
                throw new Exception("Cost price cannot be negative");

            if (dto.CostPrice > dto.Price)
                throw new Exception("Cost price cannot be greater than selling price");

            if (dto.StockQuantity < 0)
                throw new Exception("Invalid stock quantity");

            var products = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
                products.Any(x => x.Barcode == dto.Barcode))
                throw new Exception("Barcode already exists");

            var product = new Product
            {
                ProductName = dto.ProductName,
                Barcode = dto.Barcode,
                Price = dto.Price,
                CostPrice = dto.CostPrice,
                StockQuantity = dto.StockQuantity
            };

            await _repo.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var product = await _repo.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            if (string.IsNullOrWhiteSpace(dto.ProductName))
                throw new Exception("Product name is required");

            if (dto.Price <= 0)
                throw new Exception("Price must be greater than 0");

            if (dto.CostPrice < 0)
                throw new Exception("Cost price cannot be negative");

            if (dto.CostPrice > dto.Price)
                throw new Exception("Cost price cannot be greater than selling price");

            if (dto.StockQuantity < 0)
                throw new Exception("Invalid stock quantity");

            var products = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(dto.Barcode) &&
                products.Any(x => x.Barcode == dto.Barcode && x.ProductId != dto.ProductId))
                throw new Exception("Barcode already exists");

            product.ProductName = dto.ProductName;
            product.Barcode = dto.Barcode;
            product.Price = dto.Price;
            product.CostPrice = dto.CostPrice;
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
                    CostPrice = p.CostPrice,
                    StockQuantity = p.StockQuantity
                })
                .ToList();
        }
    }
}