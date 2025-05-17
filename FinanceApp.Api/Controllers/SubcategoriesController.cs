using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Infrastructure.Services.Interfaces;
using FinanceApp.Lib.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoriesController : ControllerBase
    {
        private readonly FinanceAppDbContext _context;
        private readonly ReceiptProcessor _processor;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(FinanceAppDbContext context, ICategoryService categoryService, ISubcategoryService subcategoryService)
        {
            _context = context;
            //_processor = processor;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
        }
        [HttpGet("get-subcategories-by-category-name")]
        public async Task<IEnumerable<SubcategoryDto>> GetSubcategories([FromQuery] string categoryName)
        {

            Category category = await _categoryService.GetCategoryByNameAsync(categoryName);
            var subcategories = await _subcategoryService.GetSubcategoriesByCategoryAsync(category);
            IEnumerable<SubcategoryDto> subcategoriesDtos = subcategories.Select(s => new SubcategoryDto
            {
                SubcategoryDtoId = s.SubcategoryID,
                SubcategoryDtoName = s.SubcategoryName,
                CategoryId = s.CategoryID
            });
            return subcategoriesDtos;
        } 
    }
}
