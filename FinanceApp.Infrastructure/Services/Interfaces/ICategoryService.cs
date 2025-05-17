using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Exceptions;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category> AddNewCategoryAsync(Category newCategory);
        public Task<Category> GetCategoryByIdAsync(int categoryId);
        public Task<IEnumerable<Category>> GetCategoriesAsync();
        public Task<bool> DeleteCategoryByIdAsync(int categoryId);
        public Task<Category> UpdateCategoryAsync(Category category);
        public Task<Category> GetCategoryByNameAsync(string categoryName);
    }
}
