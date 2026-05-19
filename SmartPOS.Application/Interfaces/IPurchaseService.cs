using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartPOS.Application.DTOs;

namespace SmartPOS.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task CreatePurchaseAsync(PurchaseDto dto);
    }
}
