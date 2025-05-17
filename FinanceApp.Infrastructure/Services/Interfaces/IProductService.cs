using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Product> AddNewProduct(Product newProduct);
        public Task<Product> GetProductByIdAsync(int productId);
        public Task<Product> GetProductByNameAsync(string productName);
        public Task<IEnumerable<Product>> GetProductsAsync();
        public Task<Product> UpdateProductAsync(Product product);
        public Task<bool> DeleteProductByIdAsync(Product product);
    }
}
