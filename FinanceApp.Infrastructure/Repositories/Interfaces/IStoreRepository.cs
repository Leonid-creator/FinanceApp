
using FinanceApp.Core.Entities;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IStoreRepository : IGeneralFinanceRepository<Store>
    {
        public Task<Store> GetStoreByNameAsync(string storeName);
    }
}
