namespace FinanceApp.Core.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? CategoryID { get; set; }
        public int? SubcategoryID { get; set; }
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}