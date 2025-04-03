using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceAppDbContext _context;
        public IReceiptRepository ReceiptRepository { get; }
        public IStoreRepository StoreRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ISubcategoryRepository SubcategoryRepository { get; }
        public IPurchaseDetailRepository PurchaseDetailRepository { get; }
        public UnitOfWork(FinanceAppDbContext context,
                          IReceiptRepository receiptRepo,
                          IStoreRepository storeRepo,
                          IProductRepository productRepo,
                          ICategoryRepository categoryRepo,
                          ISubcategoryRepository subcategoryRepo,
                          IPurchaseDetailRepository purchaseDetailRepo)
        {
            _context = context;
            ReceiptRepository = receiptRepo;
            StoreRepository = storeRepo;
            ProductRepository = productRepo;
            CategoryRepository = categoryRepo;
            SubcategoryRepository = subcategoryRepo;
            PurchaseDetailRepository = purchaseDetailRepo;
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public IGeneralFinanceRepository<T> GetRepository<T>() where T : class
        {
            return new GeneralFinanceRepository<T>(_context);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel)
        {
            return await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
