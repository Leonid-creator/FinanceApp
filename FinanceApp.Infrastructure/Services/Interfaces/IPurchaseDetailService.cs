using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Services.Interfaces
{
    public interface IPurchaseDetailService
    {
        public Task<PurchaseDetail> AddNewPurchaseDetailAsync(PurchaseDetail newPurchaseDetail);
        public Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails();
        public Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int receiptId, int productId);
        public Task<PurchaseDetail> UpdatePurchaseDetailAsync(PurchaseDetail purchaseDetail);
        public Task<bool> DeletePurchaseDetailByIdAsync(PurchaseDetail purchaseDetail);
    }
}
