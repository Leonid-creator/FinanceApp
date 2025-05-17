using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface ISubcategoryService
    {
        public Task<Subcategory> AddNewSubcategoryAsync(Subcategory newSubcategory);
        public Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId);
        public Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName);
        public Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();
        public Task<Subcategory> UpdateSubcategoryAsync(Subcategory subcategory);
        public Task<bool> DeleteSubcategoryByIdAsync(int subcategoryId);
        public Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryAsync(Category category);
        public Task<Subcategory> GetSubcategoryByNameAndCategoryIdAsync(string subcategoryName, int categoryId);
    }
}
