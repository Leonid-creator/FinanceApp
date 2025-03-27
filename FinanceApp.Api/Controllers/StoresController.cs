using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;

        public StoresController(FinanceAppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _context.Stores.ToListAsync();
            return Ok(stores);
        }
    }
}
