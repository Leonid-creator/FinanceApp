using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IReceiptRepository ReceiptRepository { get; }
        IStoreRepository StoreRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ISubcategoryRepository SubcategoryRepository { get; }
        IPurchaseDetailRepository PurchaseDetailRepository { get; }
        IGeneralFinanceRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel);
    }
}
