
using FinanceApp.Core.Entities;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IGeneralFinanceRepository<Product>
    {
        public Task<Product> GetProductByNameAsync(string productName);
    }
}
