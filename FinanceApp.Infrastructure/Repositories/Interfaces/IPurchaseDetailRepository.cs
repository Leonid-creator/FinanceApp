
using FinanceApp.Core.Entities;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IPurchaseDetailRepository : IGeneralFinanceRepository<PurchaseDetail>
    {
        public Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int receiptId, int productId);
        public Task<IEnumerable<PurchaseDetail>> GetPurchaseDetailsByReceiptIdAsync(int receiptId);
        public Task<bool> DeletePurchaseDetailByIdAsync(int receiptId, int productId);
    }
}
