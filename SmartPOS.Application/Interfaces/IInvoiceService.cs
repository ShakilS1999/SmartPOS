using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.Application.Interfaces
{
    public interface IInvoiceService
    {
        byte[] GenerateInvoicePdf(int saleId);
    }
}
