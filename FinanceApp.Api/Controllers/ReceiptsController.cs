using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        private readonly FinanceRepository _repository;
        private readonly ReceiptProcessor _processor;

        public ReceiptsController(FinanceRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add-test")]
        public async Task<IActionResult> AddTestReceipt([FromBody] ReceiptWithDetails receiptWithDetails)
        {
            TempReceipt tempReceipt = receiptWithDetails.tempReceipt;
            List<TempDetails> tempDetails = receiptWithDetails.tempDetails;
            await _repository.AddFullReceipt(tempReceipt, tempDetails);

            return Ok("Receipt successfully added");
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetReceipts()
        {
            List<string> products = await _repository.GetProductNamesAsync();

            return Ok(products);
        }

        [HttpGet("get-stores")]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _repository.GetStoreNamesAsync();
            return Ok(stores);
        }

    }
}
