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
        //public ReceiptsController(ReceiptProcessor processor)
        //{
        //    _processor = processor;
        //}

        [HttpPost("add-test")]
        public async Task<IActionResult> AddTestReceipt([FromBody] ReceiptWithDetails receiptWithDetails)
        {
            //_processor.TempReceipt = receiptWithDetails.tempReceipt;
            //_processor.TempDetails = receiptWithDetails.tempDetails;
            //await _processor.ProcessReceipt();

            TempReceipt tempReceipt = receiptWithDetails.tempReceipt;
            List<TempDetails> tempDetails = receiptWithDetails.tempDetails;
            await _repository.AddFullReceipt(tempReceipt, tempDetails);

            return Ok("Receipt successfully added");
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetReceipts()
        {
            //var receipts = await _context.Receipts
            //    .Include(r => r.Store)
            //    .Include(r => r.PurchaseDetails)
            //        .ThenInclude(pd => pd.Product)
            //    .ToListAsync();

            string[] products = await _repository.GetProductNamesAsync();

            return Ok(products);
        }
    }
}
