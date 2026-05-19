using SmartPOS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(SaleDto dto);
        Task<SaleDetailsDto> GetByIdAsync(int id);
        Task<List<SaleDetailsDto>> GetAllSalesAsync();
    }
}
