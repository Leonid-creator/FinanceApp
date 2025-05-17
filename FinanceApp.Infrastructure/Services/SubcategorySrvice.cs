using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;

namespace FinanceApp.Infrastructure.Services
{
    public class SubcategorySrvice : ISubcategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubcategorySrvice(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Subcategory> AddNewSubcategoryAsync(Subcategory newSubcategory)
        {
            Subcategory addedSubcategory = await _unitOfWork.SubcategoryRepository.AddEntityAsync(newSubcategory);
            await _unitOfWork.SaveChangesAsync();
            return addedSubcategory;
        }
        public async Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId)
        {
            return await _unitOfWork.SubcategoryRepository.GetByIdAsync(subcategoryId);
        }
        public async Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcategoryByNameAsync(subcategoryName);
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync()
        {
            return await _unitOfWork.SubcategoryRepository.GetAllAsync();
        }
        public async Task<Subcategory> UpdateSubcategoryAsync(Subcategory subcategory)
        {
            return await _unitOfWork.SubcategoryRepository.UpdateAsync(subcategory);
        }
        public async Task<bool> DeleteSubcategoryByIdAsync(int subcategoryId)
        {
            return await _unitOfWork.SubcategoryRepository.DeleteByIdAsync(subcategoryId);
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryAsync(Category category)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcatsByCatIdAsync(category.CategoryID);
        }
        public async Task<Subcategory> GetSubcategoryByNameAndCategoryIdAsync(string subcategoryName, int categoryId)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcategoryByNameAndCategoryIdAsync(subcategoryName, categoryId);
        }
    }
}
