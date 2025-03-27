using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly FinanceAppDbContext _context;
    private readonly FinanceRepository _repository;

    public ReceiptsController(FinanceRepository repository)
    {
        _repository = repository;
        //_context = context;
    }

    [HttpPost("add-test")]
    public async Task<IActionResult> AddTestReceipt()
    {

        var tempReceipt = new TempReceipt
        {
            StoreName = "Lidl",
            DateTime = DateTime.Now,
            TotalAmount = 123.45m,
            ReceiptDiscount = 0.0m
        };

        var tempDetails = new List<TempDetails>
        {
            new TempDetails { ProductName = "Milk", Quantity = 2, Amount = 2.5m, Discount = 0.0m, Category = "Groceries", Subcategory = "Milk" },
            new TempDetails { ProductName = "Eggs", Quantity = 1, Amount = 1.2m, Discount = 0.0m, Category = "Groceries", Subcategory = "Eggs" }
        };

        ReceiptProcessor processor = new ReceiptProcessor();
        processor.TempReceipt = tempReceipt;
        processor.TempDetails = tempDetails;
        processor.ProcessReceipt();

        //await _repository.AddFullReceipt(tempReceipt, tempDetails);

        return Ok("Receipt successfully added");
    }

    [HttpGet]
    public async Task<IActionResult> GetReceipts()
    {
        var receipts = await _context.Receipts
            .Include(r => r.Store)
            .Include(r => r.PurchaseDetails)
                .ThenInclude(pd => pd.Product)
            .ToListAsync();

        return Ok(receipts);
    }
}
