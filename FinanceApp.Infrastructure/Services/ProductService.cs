using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;

namespace FinanceApp.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Product> AddNewProduct(Product newProduct)
        {
            Product addedProduct = await _unitOfWork.ProductRepository.AddEntityAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            return addedProduct;
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        }
        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _unitOfWork.ProductRepository.GetProductByNameAsync(productName);
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
        }
        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _unitOfWork.ProductRepository.UpdateAsync(product);
        }
        public async Task<bool> DeleteProductByIdAsync(Product product)
        {
            return await _unitOfWork.ProductRepository.DeleteByIdAsync(product.ProductID);
        }
    }
}
