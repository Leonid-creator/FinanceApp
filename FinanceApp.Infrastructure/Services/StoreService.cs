using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Store> AddNewStoreAsync(StoreDto storeDto)
        {
            Store existingStore = await GetStoreByNameAsync(storeDto.StoreDtoName);
            if (existingStore == null)
            {
                Store newStore = new Store() { StoreName = storeDto.StoreDtoName };
                newStore = await _unitOfWork.StoreRepository.AddEntityAsync(newStore);
                await _unitOfWork.SaveChangesAsync();
                return newStore;
            }
            else
            {
                throw new Exception("Store with the same name already exists.");
            }

        }
        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _unitOfWork.StoreRepository.GetByIdAsync(storeId);
        }
        public async Task<Store> GetStoreByNameAsync(string storeName)
        {
            return await _unitOfWork.StoreRepository.GetStoreByNameAsync(storeName);
        }
        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            return await _unitOfWork.StoreRepository.GetAllAsync();
        }
        public async Task<Store> UpdateStoreAsync(Store store)
        {
            return await _unitOfWork.StoreRepository.UpdateAsync(store);
        }
        public async Task<bool> DeleteStoreByIdAsync(int storeId)
        {
            return await _unitOfWork.StoreRepository.DeleteByIdAsync(storeId);
        }
    }
}
