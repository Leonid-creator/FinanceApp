namespace FinanceApp.Core.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}