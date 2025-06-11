using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Infrastructure.Caching.Interfaces
{
    public interface ICategoryCache
    {
        Task InitializeAsync();
        IReadOnlyList<string> GetCategories();
        IReadOnlyList<string> GetSubcategories(string category);
        bool CategoryExists(string category);
        bool SubcategoryExists(string category, string subcategory);
    }
}
