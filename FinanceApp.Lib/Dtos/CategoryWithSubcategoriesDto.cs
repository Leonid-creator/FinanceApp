using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Lib.Dtos
{
    public class CategoryWithSubcategoriesDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<SubcategoryDto> Subcategories { get; set; }
        public  CategoryWithSubcategoriesDto()
        {
            Subcategories = new List<SubcategoryDto>();
        }
        public CategoryWithSubcategoriesDto(int categoryId, string categoryName, List<SubcategoryDto> subcategories)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Subcategories = subcategories ?? new List<SubcategoryDto>();
        }
    }
}
