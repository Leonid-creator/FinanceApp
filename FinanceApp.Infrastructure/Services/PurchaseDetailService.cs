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
    public class PurchaseDetailService : IPurchaseDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PurchaseDetail> AddNewPurchaseDetailAsync(PurchaseDetail newPurchaseDetail)
        {
            PurchaseDetail addedPurchaseDetail = await _unitOfWork.PurchaseDetailRepository.AddEntityAsync(newPurchaseDetail);
            await _unitOfWork.SaveChangesAsync();
            return addedPurchaseDetail;
        }
        public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails()
        {
            return await _unitOfWork.PurchaseDetailRepository.GetAllAsync();
        }
        public async Task<PurchaseDetail> GetPurchaseDetailByIdAsync(int receiptId, int productId)
        {
            return await _unitOfWork.PurchaseDetailRepository.GetPurchaseDetailByIdAsync(receiptId, productId);
        }
        public async Task<PurchaseDetail> UpdatePurchaseDetailAsync(PurchaseDetail purchaseDetail)
        {
            return await _unitOfWork.PurchaseDetailRepository.UpdateAsync(purchaseDetail);
        }
        public async Task<bool> DeletePurchaseDetailByIdAsync(PurchaseDetail purchaseDetail)
        {
            return await _unitOfWork.PurchaseDetailRepository.DeletePurchaseDetailByIdAsync(purchaseDetail.ReceiptID, purchaseDetail.ProductID);
        }
    }
}
