using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Lib.Dtos;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Services
{
    public class ReceiptProcessor
    {
        //private readonly FinanceAppDbContext _context;
        //private readonly FinanceRepository _repository;
        public TempReceipt TempReceipt { get; set; }
        public List<TempDetails> TempDetails { get; set; }
        public ReceiptProcessor()
        {
            TempReceipt = new TempReceipt();
            TempDetails = new List<TempDetails>();
        }
        
        public void ProcessReceipt()
        {
            if (!CheckIfStoreExist())
            {
                throw new Exception("Store does not exist!");
            }
            using (FinanceAppDbContext dbContext = new FinanceAppDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        FinanceRepository financeRepository = new FinanceRepository(dbContext);
                        financeRepository.AddFullReceipt(TempReceipt, TempDetails);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        public void CheckIfProductsExist()
        {
            using (FinanceAppDbContext dbContext = new FinanceAppDbContext())
            {
                for (int i = 0; i < TempDetails.Count; i++)
                {
                    if (dbContext.Products.FirstOrDefault(p => p.ProductName == TempDetails[i].ProductName) == null)
                    {
                        //Console.WriteLine($"Product \"{TempDetails[i].ProductName}\" not found in database");
                        if (string.IsNullOrEmpty(TempDetails[i].Category))
                        {
                            Console.WriteLine($"Category name for \"{TempDetails[i].ProductName}\":");
                            TempDetails[i].Category = Console.ReadLine();
                            if (TempDetails[i].Category == string.Empty)
                            {
                                TempDetails[i].Category = "UNCATEGORIZED";
                            }
                        }
                        CheckIfCategoryExist(i);
                        if (string.IsNullOrEmpty(TempDetails[i].Subcategory))
                        {
                            Console.WriteLine($"Subcategory name for \"{TempDetails[i].ProductName}\":");
                            TempDetails[i].Subcategory = Console.ReadLine();
                            if (TempDetails[i].Subcategory == string.Empty)
                            {
                                TempDetails[i].Subcategory = "UNCATEGORIZED";
                            }
                        }
                        CheckIfSubcategoryExist(i);
                    }
                }
            }
        }
        public bool CheckIfStoreExist()
        {
            using (FinanceAppDbContext dbContext = new FinanceAppDbContext())
            {
                if (dbContext.Stores.FirstOrDefault(s => s.StoreName == TempReceipt.StoreName) != null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Unknown store \"{TempReceipt.StoreName}\"");
                    Console.WriteLine("Add new store? (y/n)");
                    string option = Console.ReadLine();
                    if (option == "y")
                    {
                        FinanceRepository financeRepository = new FinanceRepository(dbContext);
                        financeRepository.AddNewStore(TempReceipt.StoreName);
                        return true;
                    }
                    return false;
                }
            }
        }
        public bool CheckIfCategoryExist(int prodNum)
        {
            using (FinanceAppDbContext dbContext = new FinanceAppDbContext())
            {
                //for (int i = 0; i < TempDetails.Count; i++)
                //{
                if (dbContext.Categories.FirstOrDefault(c => c.CategoryName == TempDetails[prodNum].Category) == null)
                {
                    Console.WriteLine($"Unknown category \"{TempDetails[prodNum].Category}\"");
                    Console.WriteLine("Add new category? (y/n)");
                    string option = Console.ReadLine();
                    if (option == "y")
                    {
                        FinanceRepository financeRepository = new FinanceRepository(dbContext);
                        financeRepository.AddNewCategory(TempDetails[prodNum].Category);
                    }
                    else
                    {
                        return false;
                    }
                }
                //}
                return true;
            }
        }
        public bool CheckIfSubcategoryExist(int prodNum)
        {
            using (FinanceAppDbContext dbContext = new FinanceAppDbContext())
            {
                FinanceRepository financeRepository = new FinanceRepository(dbContext);
                //for (int i = 0; i < TempDetails.Count; i++)
                //{
                Category category = dbContext.Categories.FirstOrDefault(c => c.CategoryName == TempDetails[prodNum].Category);
                if (category == null)
                {
                    throw new NullReferenceException($"Unknown category \"{TempDetails[prodNum].Category}\"");
                }
                Subcategory subcategory = dbContext.Subcategories.FirstOrDefault(s => s.SubcategoryName == TempDetails[prodNum].Subcategory && s.CategoryID == category.CategoryID);
                if (subcategory == null)
                {
                    Console.WriteLine($"Unknown subcategory \"{TempDetails[prodNum].Subcategory}\"");
                    Console.WriteLine("Add new subcategory? (y/n)");
                    string option = Console.ReadLine();
                    if (option == "y")
                    {
                        financeRepository.AddNewSubcategory(TempDetails[prodNum].Category, TempDetails[prodNum].Subcategory);
                    }
                    else
                    {
                        return false;
                    }
                }
                //}
                return true;
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
