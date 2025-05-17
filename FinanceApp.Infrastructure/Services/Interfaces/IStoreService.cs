using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface IStoreService
    {
        public Task<Store> AddNewStoreAsync(StoreDto newStore);
        public Task<Store> GetStoreByIdAsync(int storeId);
        public Task<Store> GetStoreByNameAsync(string storeName);
        public Task<IEnumerable<Store>> GetStoresAsync();
        public Task<Store> UpdateStoreAsync(Store store);
        public Task<bool> DeleteStoreByIdAsync(int storeId);
    }
}
