using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Repositories
{
    public class ProductRepository : GeneralFinanceRepository<Product>, IProductRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public ProductRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Product> GetProductByNameAsync(string productName)
        {
            throw new NotImplementedException();
        }
    }
}
