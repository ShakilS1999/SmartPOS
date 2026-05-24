using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task CreatePurchaseAsync(PurchaseDto dto);
        Task<List<PurchaseDetailsDto>> GetAllPurchasesAsync();
    }
}