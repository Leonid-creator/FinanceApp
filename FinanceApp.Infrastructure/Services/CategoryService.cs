using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Exceptions;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> AddNewCategoryAsync(Category newCategory)
        {
            try
            {
                Category category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(newCategory.CategoryName);
                if (category != null)
                {
                    throw new CategoryAlreadyExistsException($"Category with name \"{newCategory.CategoryName}\" already exists");
                }
                else
                {
                    category = await _unitOfWork.CategoryRepository.AddEntityAsync(newCategory);
                    return category;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category to database", ex);
            }
        }
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting category by ID from database", ex);
            }
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting categories from database", ex);
            }
        }
        public async Task<bool> DeleteCategoryByIdAsync(int categoryId)
        {
            try
            {
                Category category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return false;
                }
                else
                {
                    return await _unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category by ID from database", ex);
            }
        }
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            try
            {
                Category existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(category.CategoryID);
                if (existingCategory == null)
                {
                    throw new NullReferenceException($"Category with ID {category.CategoryID} not found");
                }
                else
                {
                    return await _unitOfWork.CategoryRepository.UpdateAsync(category);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category", ex);
            }
        }
        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(categoryName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting category by name from database", ex);
            }
        }
    }
}
