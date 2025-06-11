using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceApp.Infrastructure.Caching.Interfaces;

namespace FinanceApp.Infrastructure.Caching
{
    public class CategoryCache : ICategoryCache
    {
        private readonly Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();
        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
        public IReadOnlyList<string> GetCategories()
        {
            throw new NotImplementedException();
        }
        public IReadOnlyList<string> GetSubcategories(string category)
        {
            throw new NotImplementedException();
        }
        public bool CategoryExists(string category)
        {
            throw new NotImplementedException();
        }
        public bool SubcategoryExists(string category, string subcategory)
        {
            throw new NotImplementedException();
        }
    }
}
