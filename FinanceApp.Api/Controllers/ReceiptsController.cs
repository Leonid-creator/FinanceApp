using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly FinanceRepository _repository;
        private readonly ReceiptProcessor _processor;

        public ReceiptsController(ReceiptProcessor processor, FinanceRepository repository)
        {
            _processor = processor;
            _repository = repository;
        }

        [HttpPost("add-full-receipt")]
        public async Task<IActionResult> AddFullReceipt([FromBody] ReceiptWithDetails receiptWithDetails)
        {
            _processor.TempReceipt = receiptWithDetails.tempReceipt;
            _processor.TempDetails = receiptWithDetails.tempDetails;
            await _processor.ProcessReceipt();

            return Ok("Receipt successfully added");
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetReceipts()
        {
            List<string> products = await _repository.GetProductNamesAsync();

            return Ok(products);
        }
    }
}
