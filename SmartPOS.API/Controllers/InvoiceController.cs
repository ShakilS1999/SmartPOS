using Microsoft.AspNetCore.Mvc;
using SmartPOS.Application.Interfaces;

namespace SmartPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet("{saleId}")]
        public IActionResult DownloadInvoice(int saleId)
        {
            var pdf = _service.GenerateInvoicePdf(saleId);

            return File(
                pdf,
                "application/pdf",
                $"Invoice-{saleId}.pdf"
            );
        }
    }
}