
namespace FinanceApp.Client
{
    public class ProductEntryFields
    {
        public Entry Quantity { get; set; }
        public Entry ProductName { get; set; }
        public Entry Amount { get; set; }
        public Entry Discount { get; set; }
        public Picker Category { get; set; }
        public Picker Subcategory { get; set; }
    }
}
