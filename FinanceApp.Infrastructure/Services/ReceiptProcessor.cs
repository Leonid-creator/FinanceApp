using FinanceApp.Core.Entities;
using FinanceApp.Lib.Dtos;
using FinanceApp.Infrastructure.Exceptions;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Services
{
    public class ReceiptProcessor
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<TempDetails> TempDetails;
        public TempReceipt TempReceipt;
        public ReceiptProcessor(IUnitOfWork unitOfWork, TempReceipt tempReceipt, List<TempDetails> tempDetails)
        {
            _unitOfWork = unitOfWork;
            TempDetails = tempDetails;
            TempReceipt = tempReceipt;
        }
        public async Task ProcessReceipt() 
        {
            try
            {
                //using (var transaction = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                //{
                    CheckTotalAmount();
                    Store store = await GetStoreByNameAsync(TempReceipt.StoreName);
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
                        await AddNewReceiptAsync(receipt);
                        for (int i = 0; TempDetails.Count() > i; i++)
                        {
                            Product product = await GetProductByNameAsync(TempDetails[i].ProductName);
                            if (product == null)
                            {
                                product = new Product();
                                Category category = await GetCategoryByNameAsync(TempDetails[i].Category);
                                if (category == null)
                                {
                                    throw new CategoryNotFoundException($"Category with name \"{TempDetails[i].Category}\" not found");
                                }
                                Subcategory subcategory = await GetSubcategoryByNameAndCategoryIdAsync(TempDetails[i].Subcategory, category.CategoryID);
                                if (subcategory == null || subcategory.CategoryID != category.CategoryID)
                                {
                                    throw new SubcategoryNotFoundException($"Subcategory with name \"{TempDetails[i].Subcategory}\" not found");
                                }
                                product.ProductName = TempDetails[i].ProductName;
                                product.CategoryID = category.CategoryID;
                                product.SubcategoryID = subcategory.SubcategoryID;
                                await AddNewProduct(product);
                            }
                            PurchaseDetail purchaseDetail = new PurchaseDetail
                            {
                                ReceiptID = receipt.ReceiptID,
                                ProductID = product.ProductID,
                                Quantity = TempDetails[i].Quantity,
                                Amount = TempDetails[i].Amount,
                                Discount = TempDetails[i].Discount
                            };
                            await AddNewPurchaseDetailAsync(purchaseDetail);
                        }
                    }
                    await _unitOfWork.SaveChangesAsync();
                    //await transaction.CommitAsync();
                //}
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

        //--------------------Category----------------------- +
        public async Task<Category> AddNewCategoryAsync(Category newCategory)
        {
            try
            {
                Category category = await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(newCategory.CategoryName);
                if (category != null)
                {
                    throw new CategoryAlreadyExistsException($"Category with name \"{newCategory.CategoryName}\" already exists");
                }
                else
                {
                    category = await _unitOfWork.CategoryRepository.AddEntityAsync(newCategory);
                    return category;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category to database", ex);
            }
        }
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting category by ID from database", ex);
            }
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting categories from database", ex);
            }
        }
        public async Task<bool> DeleteCategoryByIdAsync(int categoryId)
        {
            try
            {
                Category category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return false;
                }
                else
                {
                    return await _unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category by ID from database", ex);
            }
        }
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            try
            {
                Category existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(category.CategoryID);
                if (existingCategory == null)
                {
                    throw new NullReferenceException($"Category with ID {category.CategoryID} not found");
                }
                else
                {
                    return await _unitOfWork.CategoryRepository.UpdateAsync(category);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category", ex);
            }
        }
        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                return await _unitOfWork.CategoryRepository.GetCategoryByNameAsync(categoryName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting category by name from database", ex);
            }
        }


        //--------------------Product------------------------
        public async Task<Product> AddNewProduct(Product newProduct)
        {
            Product addedProduct = await _unitOfWork.ProductRepository.AddEntityAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            return addedProduct;
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(productId);
        }
        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _unitOfWork.ProductRepository.GetProductByNameAsync(productName);
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
        }
        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _unitOfWork.ProductRepository.UpdateAsync(product);
        }
        public async Task<bool> DeleteProductByIdAsync(Product product)
        {
            return await _unitOfWork.ProductRepository.DeleteByIdAsync(product.ProductID);
        }


        //--------------------PurchaseDetail-----------------
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


        //--------------------Receipt------------------------
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


        //--------------------Store--------------------------
        public async Task<Store> AddNewStoreAsync(Store newStore)
        {
            Store addedStore = await _unitOfWork.StoreRepository.AddEntityAsync(newStore);
            await _unitOfWork.SaveChangesAsync();
            return addedStore;
        }
        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _unitOfWork.StoreRepository.GetByIdAsync(storeId);
        }
        public async Task<Store> GetStoreByNameAsync(string storeName)
        {
            return await _unitOfWork.StoreRepository.GetStoreByNameAsync(storeName);
        }
        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            return await _unitOfWork.StoreRepository.GetAllAsync();
        }
        public async Task<Store> UpdateStoreAsync(Store store)
        {
            return await _unitOfWork.StoreRepository.UpdateAsync(store);
        }
        public async Task<bool> DeleteStoreByIdAsync(int storeId)
        {
            return await _unitOfWork.StoreRepository.DeleteByIdAsync(storeId);
        }


        //--------------------Subcategory--------------------
        public async Task<Subcategory> AddNewSubcategoryAsync(Subcategory newSubcategory)
        {
            Subcategory addedSubcategory = await _unitOfWork.SubcategoryRepository.AddEntityAsync(newSubcategory);
            await _unitOfWork.SaveChangesAsync();
            return addedSubcategory;
        }
        public async Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId)
        {
            return await _unitOfWork.SubcategoryRepository.GetByIdAsync(subcategoryId);
        }
        public async Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcategoryByNameAsync(subcategoryName);
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync()
        {
            return await _unitOfWork.SubcategoryRepository.GetAllAsync();
        }
        public async Task<Subcategory> UpdateSubcategoryAsync(Subcategory subcategory)
        {
            return await _unitOfWork.SubcategoryRepository.UpdateAsync(subcategory);
        }
        public async Task<bool> DeleteSubcategoryByIdAsync(int subcategoryId)
        {
            return await _unitOfWork.SubcategoryRepository.DeleteByIdAsync(subcategoryId);
        }
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryAsync(Category category)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcatsByCatIdAsync(category.CategoryID);
        }
        public async Task<Subcategory> GetSubcategoryByNameAndCategoryIdAsync (string subcategoryName, int categoryId)
        {
            return await _unitOfWork.SubcategoryRepository.GetSubcategoryByNameAndCategoryIdAsync(subcategoryName, categoryId);
        }

    }
}