using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
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
        public SubcategoriesController(FinanceAppDbContext context, ReceiptProcessor processor)
        {
            _context = context;
            _processor = processor;
        }
        [HttpGet("get-subcategories-by-category-name")]
        public async Task<IEnumerable<SubcategoryDto>> GetSubcategories([FromQuery] string categoryName)
        {

            Category category = await _processor.GetCategoryByNameAsync(categoryName);
            var subcategories = await _processor.GetSubcategoriesByCategoryAsync(category);
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
