using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        private readonly ReceiptProcessor _processor;


        public StoresController(FinanceAppDbContext context, ReceiptProcessor processor)
        {
            _context = context;
            _processor = processor;
        }
        [HttpPost("add-store")]
        public async Task<Store> AddNewStoreAsync([FromBody] StoreDto storeDto)
        {
            Store newStore = new Store() { StoreName = storeDto.StoreDtoName };
            return await _processor.AddNewStoreAsync(newStore);
        }

        [HttpGet("get-stores")]
        public async Task<IEnumerable<StoreDto>> GetStores()
        {
            var stores = await _processor.GetStoresAsync();
            IEnumerable<StoreDto> storeDtos = stores.Select(s => new StoreDto
            {
                StoreDtoId = s.StoreID,
                StoreDtoName = s.StoreName
            });
            return storeDtos;
        }
    }
}
