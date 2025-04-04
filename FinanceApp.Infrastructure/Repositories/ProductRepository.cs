using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                Product product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
                return product;
            }
            catch
            {
                throw new Exception("Error getting product from database");
            }
        }
    }
}
