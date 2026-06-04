using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(SaleDto dto);

        Task<SaleDetailsDto?> GetByIdAsync(int id);

        Task<List<SaleDetailsDto>> GetAllSalesAsync();
    }
}
