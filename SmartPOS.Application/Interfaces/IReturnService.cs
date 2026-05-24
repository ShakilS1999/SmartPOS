using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface IReturnService
    {
        Task CreateReturnAsync(ReturnDto dto);
        Task<List<ReturnDetailsDto>> GetAllReturnsAsync();
    }
}