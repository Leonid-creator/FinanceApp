
using FinanceApp.Core.Entities;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IGeneralFinanceRepository<Category>
    {
        public Task<Category> GetCategoryByNameAsync(string categoryName);
    }
}
