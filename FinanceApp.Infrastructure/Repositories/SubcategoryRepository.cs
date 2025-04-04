using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class SubcategoryRepository : GeneralFinanceRepository<Subcategory>, ISubcategoryRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public SubcategoryRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName)
        {
            try
            {
                return await _dbContext.Subcategories.FirstOrDefaultAsync(s => s.SubcategoryName == subcategoryName);
            }
            catch
            {
                throw new Exception("Error getting subcategory by name from database");
            }
        }
        public async Task<Subcategory> GetSubcategoryByNameAndCategoryIdAsync(string subcategoryName, int categoryId)
        {
            try
            {
                return await _dbContext.Subcategories.FirstOrDefaultAsync(s => s.SubcategoryName == subcategoryName && s.CategoryID == categoryId);
            }
            catch
            {
                throw new Exception("Error getting subcategory by name from database");
            }
        }
        public async Task<IEnumerable<Subcategory>> GetSubcatsByCatIdAsync(int categoryId)
        {
            try
            {
                IEnumerable<Subcategory> subcategories= await _dbContext.Subcategories.Where(s => s.CategoryID == categoryId).ToListAsync();
                return subcategories;
            }
            catch
            {
                throw new Exception("Error getting store from database");
            }
        }
        public async Task<bool> DeleteSubcategoryByIdAsync(int subcategoryId)
        {
            try
            {
                Subcategory subcategory = await _dbContext.Subcategories.FindAsync(subcategoryId);
                if (subcategory == null)
                {
                    return false;
                }
                else
                {
                    _dbContext.Subcategories.Remove(subcategory);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                throw new Exception("Error deleting subcategory from database");
            }
        }
    }
}
