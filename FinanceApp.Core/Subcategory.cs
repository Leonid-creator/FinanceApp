namespace FinanceApp.Core
{
    public class Subcategory
    {
        public int SubcategoryID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public ICollection<Product> Products { get; set; }
        public Category Category { get; set; }
    }
}
