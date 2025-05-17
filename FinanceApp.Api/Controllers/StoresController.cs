using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;
using FinanceApp.Infrastructure.Services.Interfaces;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        private readonly IStoreService _storeService;

        public StoresController(FinanceAppDbContext context, IStoreService service)
        {
            _context = context;
            _storeService = service;
        }

        [HttpPost("add-store")]
        public async Task<Store> AddNewStoreAsync([FromBody] StoreDto storeDto)
        {
            //Store newStore = new Store() { StoreName = storeDto.StoreDtoName };
            return await _storeService.AddNewStoreAsync(storeDto);
        }

        [HttpGet("get-stores")]
        public async Task<IEnumerable<StoreDto>> GetStores()
        {
            var stores = await _storeService.GetStoresAsync();
            IEnumerable<StoreDto> storeDtos = stores.Select(s => new StoreDto
            {
                StoreDtoId = s.StoreID,
                StoreDtoName = s.StoreName
            });
            return storeDtos;
        }
    }
}
