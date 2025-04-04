using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class StoreRepository : GeneralFinanceRepository<Store>, IStoreRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public StoreRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Store> GetStoreByNameAsync(string storeName)
        {
            try
            {
                Store store = await _dbContext.Stores.FirstOrDefaultAsync(s => s.StoreName == storeName);
                return store;
            }
            catch
            {
                throw new Exception("Error getting store from database");
            }
        }
    }
}
