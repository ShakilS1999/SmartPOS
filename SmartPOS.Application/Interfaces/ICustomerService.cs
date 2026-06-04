using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();

        Task<CustomerDto?> GetByIdAsync(int id);

        Task CreateAsync(CustomerDto dto);

        Task UpdateAsync(CustomerDto dto);

        Task DeleteAsync(int id);
    }
}
