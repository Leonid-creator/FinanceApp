using FinanceApp.Core.Entities;
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
        private readonly FinanceRepository _repository;
        private readonly ReceiptProcessor _processor;

        public ReceiptsController(ReceiptProcessor processor, FinanceRepository repository)
        {
            _processor = processor;
            _repository = repository;
        }

        [HttpPost("add-test")]
        public async Task<IActionResult> AddTestReceipt([FromBody] ReceiptWithDetails receiptWithDetails)
        {
            TempReceipt tempReceipt = receiptWithDetails.tempReceipt;
            List<TempDetails> tempDetails = receiptWithDetails.tempDetails;
            //await _repository.AddFullReceipt(tempReceipt, tempDetails);

            //_processor.TempReceipt = tempReceipt;
            //_processor.TempDetails = tempDetails;
            await _processor.ProcessReceipt();

            return Ok("Receipt successfully added");
        }

        [HttpPost("add-store")]
        public async Task<Store> AddNewStoreAsync([FromBody] StoreDto storeDto)
        {
            Store newStore = new Store() { StoreName = storeDto.StoreDtoName };
            return await _processor.AddNewStoreAsync(newStore);
            // Ok("Store successfully added");
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
            StoreDto retStores = new StoreDto() { StoreDtoName = stores.First() };
            return Ok(retStores);
        }
    }
}
