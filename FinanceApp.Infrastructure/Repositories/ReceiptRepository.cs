using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Lib.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class ReceiptRepository : GeneralFinanceRepository<Receipt>, IReceiptRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public ReceiptRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Receipt> GetReceiptByDetails(TempReceipt tempReceipt, int storeId)
        {
            Receipt receipt = await _dbContext.Receipts.FirstOrDefaultAsync(r => r.StoreID == storeId &&
                                                                             r.TotalAmount == tempReceipt.TotalAmount &&
                                                                             r.DateTime == tempReceipt.DateTime);
            return receipt;
        }
    }
}
