using FinanceApp.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using FinanceApp.Lib.Dtos;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories
{
    public class FinanceRepository
    {
        readonly FinanceAppDbContext DbContext;
        public FinanceRepository(FinanceAppDbContext context)
        {
            DbContext = context;
        }

        public Task AddFullReceipt(TempReceipt tempReceipt, List<TempDetails> tempDetails)
        {
            int receiptID = AddBriefReceiptInfo(tempReceipt);
            if (receiptID == 0)
            {
                throw new Exception("ReceiptID is 0, something went wrong");
            }
            for (int i = 0; i < tempDetails.Count; i++)
            {
                AddPurchaseDetail(tempDetails[i], receiptID);
            }
            DbContext.SaveChanges();
            return Task.CompletedTask;
        }
        private int AddBriefReceiptInfo(TempReceipt tempReceipt)
        {
            if (tempReceipt.StoreName.IsNullOrEmpty() || tempReceipt.TotalAmount == null || tempReceipt.DateTime == null || tempReceipt.ReceiptDiscount == null)
            {
                throw new ArgumentNullException("AddBriefReceiptInfo() cannot accept \"null\"");
            }
            else
            {
                Store store = DbContext.Stores.FirstOrDefault(s => s.StoreName == tempReceipt.StoreName);
                if (store == null)
                {
                    throw new Exception("Unknown store name!");
                }

                Receipt newReceipt = new Receipt()
                {
                    StoreID = store.StoreID,
                    DateTime = tempReceipt.DateTime,
                    TotalAmount = tempReceipt.TotalAmount,
                    ReceiptDiscount = tempReceipt.ReceiptDiscount
                };
                var existingReceipt = DbContext.Receipts.FirstOrDefault(r => r.StoreID == store.StoreID
                                                                        && r.DateTime == newReceipt.DateTime
                                                                        && r.TotalAmount == newReceipt.TotalAmount);

                if (existingReceipt == null)
                {
                    DbContext.Receipts.Add(newReceipt);
                    DbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Receipt already exists!");
                }
                return newReceipt.ReceiptID;
            }
        }
        private void AddPurchaseDetail(TempDetails tempDetails, int receiptID)
        {
            if (tempDetails.ProductName == null || tempDetails.Quantity == null || tempDetails.Amount == null)
            {
                throw new ArgumentNullException("Exception occurred in AddPurchaseDetail");
            }
            else
            {
                Product newProduct = DbContext.Products.FirstOrDefault(s => s.ProductName == tempDetails.ProductName);
                if (newProduct == null)
                {
                    newProduct = AddProduct(tempDetails);
                }

                PurchaseDetail newPurchaseDetail = new PurchaseDetail
                {
                    ReceiptID = receiptID,
                    ProductID = newProduct.ProductID,
                    Quantity = tempDetails.Quantity,
                    Amount = tempDetails.Amount,
                    Discount = tempDetails.Discount
                };
                DbContext.PurchaseDetails.Add(newPurchaseDetail);
            }
        }
        public Store AddNewStore(string storeName)
        {
            if (storeName == null)
            {
                throw new ArgumentNullException("Exception occurred in AddStore");
            }
            else
            {
                Store newStore = new Store { StoreName = storeName };
                DbContext.Stores.Add(newStore);
                DbContext.SaveChanges();
                return newStore;
            }
        }
        private Product AddProduct(TempDetails tempDetails)
        {
            if (tempDetails.ProductName == null || tempDetails.Category == null || tempDetails.Subcategory == null)
            {
                throw new ArgumentNullException("Exception occurred in AddProduct");
            }
            else
            {
                Category category = DbContext.Categories.FirstOrDefault(c => c.CategoryName == tempDetails.Category);
                Subcategory subcategory = DbContext.Subcategories.FirstOrDefault(s => s.SubcategoryName == tempDetails.Subcategory && s.CategoryID == category.CategoryID);
                if (subcategory == null)
                {
                    throw new Exception($"Unknown subcategory \"{tempDetails.Subcategory}\" with category \"{tempDetails.Category}\"!");
                }

                Product newProduct = new Product
                {
                    ProductName = tempDetails.ProductName,
                    SubcategoryID = subcategory.SubcategoryID,
                    CategoryID = subcategory.CategoryID
                };
                DbContext.Products.Add(newProduct);
                DbContext.SaveChanges();
                return newProduct;
            }
        }
        public Subcategory AddNewSubcategory(string category, string subcategory)
        {
            if (subcategory == null || category == null)
            {
                throw new ArgumentNullException("Exception occurred in AddSubcategory");
            }
            else
            {
                Category newCategory = DbContext.Categories.FirstOrDefault(c => c.CategoryName == category);
                if (newCategory == null)
                {
                    throw new Exception("Unknown category!");
                }

                Subcategory newSubcategory = new Subcategory
                {
                    SubcategoryName = subcategory,
                    CategoryID = newCategory.CategoryID
                };
                DbContext.Subcategories.Add(newSubcategory);
                DbContext.SaveChanges();
                return newSubcategory;
            }
        }
        public Category AddNewCategory(string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Exception occurred in AddCategory");
            }
            else
            {
                Category newCategory = new Category { CategoryName = category };
                DbContext.Categories.Add(newCategory);
                DbContext.SaveChanges();
                return newCategory;
            }
        }

        public Task<List<string>> GetProductNamesAsync()
        {
            return DbContext.Products
                .Select(p => p.ProductName)
                .Distinct()
                .ToListAsync();
        }
        public Task<string[]> GetStoreNamesAsync()
        {
            return DbContext.Stores
                .Select(s => s.StoreName)
                .Distinct()
                .ToArrayAsync();
        }
    }
}
