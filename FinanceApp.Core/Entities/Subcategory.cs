namespace FinanceApp.Core.Entities
{
    public class Subcategory
    {
        public int SubcategoryID { get; set; }
        public string SubcategoryName { get; set; }
        public int CategoryID { get; set; }
        public ICollection<Product> Products { get; set; }
        public Category Category { get; set; }
    }
}
