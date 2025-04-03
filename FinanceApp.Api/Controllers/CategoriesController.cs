using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        private readonly ReceiptProcessor _processor;
        public CategoriesController(FinanceAppDbContext context, ReceiptProcessor processor)
        {
            _context = context;
            _processor = processor;
        }
        [HttpGet("get-categories")]
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _processor.GetCategoriesAsync();
            IEnumerable<CategoryDto> categoriesDtos = categories.Select(c => new CategoryDto
            {
                CategoryDtoId = c.CategoryID,
                CategoryDtoName = c.CategoryName
            });
            return categoriesDtos;
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategoryAsync(CategoryDto categoryDto)
        {
            Category category = new Category
            {
                CategoryID = categoryDto.CategoryDtoId,
                CategoryName = categoryDto.CategoryDtoName
            };
            await _processor.AddNewCategoryAsync(category);
            return Ok();
        }
    }
}
