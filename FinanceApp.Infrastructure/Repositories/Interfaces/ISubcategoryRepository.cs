
using FinanceApp.Core.Entities;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface ISubcategoryRepository : IGeneralFinanceRepository<Subcategory>
    {
        public Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName);
        public Task<Subcategory> GetSubcategoryByNameAndCategoryIdAsync(string subcategoryName, int categoryId);
        public Task<IEnumerable<Subcategory>> GetSubcatsByCatIdAsync(int categoryId);
        public Task<bool> DeleteSubcategoryByIdAsync(int subcategoryId);
    }
}
