using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class CategoryRepository : GeneralFinanceRepository<Category>, ICategoryRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public CategoryRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                return await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            }
            catch
            {
                throw new Exception("Error getting category by name from database");
            }
        }
    }
}
