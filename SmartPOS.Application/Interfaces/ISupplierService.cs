using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllAsync();
        Task CreateAsync(SupplierDto dto);
        Task UpdateAsync(SupplierDto dto);
        Task DeleteAsync(int id);
    }
}