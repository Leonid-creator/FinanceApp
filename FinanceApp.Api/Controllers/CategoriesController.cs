using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Infrastructure.Services.Interfaces;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        //private readonly ReceiptProcessor _processor;
        private readonly ICategoryService _categoryService;
        public CategoriesController(FinanceAppDbContext context, ICategoryService categoryService)
        {
            _context = context;
            //_processor = processor;
            _categoryService = categoryService;
        }
        [HttpGet("get-categories")]
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
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
            await _categoryService.AddNewCategoryAsync(category);
            return Ok();
        }
    }
}
