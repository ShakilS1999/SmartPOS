using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task CreateAsync(ProductDto dto);

        Task UpdateAsync(ProductDto dto);

        Task DeleteAsync(int id);

        Task<List<ProductDto>> GetLowStockAsync();
    }
}
