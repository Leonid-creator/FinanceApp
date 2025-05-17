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
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReceiptService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Receipt> AddNewReceiptAsync(Receipt newReceipt)
        {
            Receipt addedReceipt = await _unitOfWork.ReceiptRepository.AddEntityAsync(newReceipt);
            await _unitOfWork.SaveChangesAsync();
            return addedReceipt;
        }
        public async Task<Receipt> GetReceiptByIdAsync(int receiptId)
        {
            return await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
        }
        public async Task<IEnumerable<Receipt>> GetReceiptsAsync()
        {
            return await _unitOfWork.ReceiptRepository.GetAllAsync();
        }
        public async Task<Receipt> UpdateReceiptAsync(Receipt receipt)
        {
            return await _unitOfWork.ReceiptRepository.UpdateAsync(receipt);
        }
        public async Task<bool> DeleteReceiptByIdAsync(Receipt receipt)
        {
            return await _unitOfWork.ReceiptRepository.DeleteByIdAsync(receipt.ReceiptID);
        }
    }
}
