using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Lib.Dtos;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface IReceiptService
    {
        public Task<Receipt> AddNewReceiptAsync(Receipt newReceipt);
        public Task<Receipt> GetReceiptByIdAsync(int receiptId);
        public Task<IEnumerable<Receipt>> GetReceiptsAsync();
        public Task<Receipt> UpdateReceiptAsync(Receipt receipt);
        public Task<bool> DeleteReceiptByIdAsync(Receipt receipt);
        public Task<bool> IfReceiptExist(TempReceipt tempReceipt, int storeId);
    }
}
