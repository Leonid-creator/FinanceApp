using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Repositories
{
    public class PurchaseDetailRepository : GeneralFinanceRepository<PurchaseDetail>, IPurchaseDetailRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public PurchaseDetailRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int receiptId, int productId)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetailsByReceiptIdAsync(int receiptId)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeletePurchaseDetailByIdAsync(int receiptId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
