using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task CreateAsync(CreateUserDto dto);
        Task DeleteAsync(int id);
    }
}