using FinanceApp.Core.Entities;
using FinanceApp.Lib.Dtos;
using FinanceApp.Infrastructure.Exceptions;
using FinanceApp.Infrastructure.Repositories.Interfaces;
using FinanceApp.Infrastructure.Services.Interfaces;

namespace FinanceApp.Infrastructure.Services
{
    public class ReceiptProcessor
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<TempDetails> TempDetails;
        public TempReceipt TempReceipt;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IPurchaseDetailService _purchaseDetailService;
        private readonly IReceiptService _receiptService;
        private readonly IStoreService _storeService;
        private readonly ISubcategoryService _subcategoryService;
        public ReceiptProcessor(
            IUnitOfWork unitOfWork, 
            TempReceipt tempReceipt, 
            List<TempDetails> tempDetails,
            ICategoryService categoryService,
            IProductService productService,
            IPurchaseDetailService purchaseDetailService,
            IReceiptService receiptService,
            IStoreService storeService,
            ISubcategoryService subcategoryService)
        {
            _unitOfWork = unitOfWork;
            TempDetails = tempDetails;
            TempReceipt = tempReceipt;
            _categoryService = categoryService;
            _productService = productService;
            _purchaseDetailService = purchaseDetailService;
            _receiptService = receiptService;
            _storeService = storeService;
            _subcategoryService = subcategoryService;
        }
        public async Task ProcessReceipt() 
        {
            try
            {
                CheckTotalAmount();
                
                Store store = await _storeService.GetStoreByNameAsync(TempReceipt.StoreName);
                if (store == null)
                {
                    throw new StoreNotFoundException($"Store with name \"{TempReceipt.StoreName}\" not found");
                }
                else
                {
                    Receipt receipt = new Receipt
                    {
                        StoreID = store.StoreID,
                        DateTime = TempReceipt.DateTime,
                        TotalAmount = TempReceipt.TotalAmount,
                        ReceiptDiscount = TempReceipt.ReceiptDiscount
                    };
                    await _receiptService.AddNewReceiptAsync(receipt);
                    for (int i = 0; TempDetails.Count() > i; i++)
                    {
                        Product product = await _productService.GetProductByNameAsync(TempDetails[i].ProductName);
                        if (product == null)
                        {
                            product = new Product();
                            Category category = await _categoryService.GetCategoryByNameAsync(TempDetails[i].Category);
                            if (category == null)
                            {
                                throw new CategoryNotFoundException($"Category with name \"{TempDetails[i].Category}\" not found");
                            }
                            Subcategory subcategory = await _subcategoryService.GetSubcategoryByNameAndCategoryIdAsync(TempDetails[i].Subcategory, category.CategoryID);
                            if (subcategory == null || subcategory.CategoryID != category.CategoryID)
                            {
                                throw new SubcategoryNotFoundException($"Subcategory with name \"{TempDetails[i].Subcategory}\" not found");
                            }
                            product.ProductName = TempDetails[i].ProductName;
                            product.CategoryID = category.CategoryID;
                            product.SubcategoryID = subcategory.SubcategoryID;
                            await _productService.AddNewProduct(product);
                        }
                        PurchaseDetail purchaseDetail = new PurchaseDetail
                        {
                            ReceiptID = receipt.ReceiptID,
                            ProductID = product.ProductID,
                            Quantity = TempDetails[i].Quantity,
                            Amount = TempDetails[i].Amount,
                            Discount = TempDetails[i].Discount
                        };
                        await _purchaseDetailService.AddNewPurchaseDetailAsync(purchaseDetail);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch 
            {
                throw new Exception("Error processing receipt");
            }
        }
        public void CheckTotalAmount() 
        {
            decimal sumAmountByDetails = TempDetails.Sum(s => s.Amount * s.Quantity + s.Discount);
            decimal diffAmount = TempReceipt.TotalAmount - TempReceipt.ReceiptDiscount - sumAmountByDetails;
            if (diffAmount != 0)
            {
                throw new WrongAmountException($"Error! Total amount are not equal to sum of amounts in Details ({diffAmount})");
            }
        }
    }
}