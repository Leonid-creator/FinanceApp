
using FinanceApp.Core.Entities;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IReceiptRepository : IGeneralFinanceRepository<Receipt>
    {
        public Task<Receipt> GetReceiptByDetails(TempReceipt tempReceipt, int storeId);
    }
}
