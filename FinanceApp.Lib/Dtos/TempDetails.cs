
namespace FinanceApp.Lib.Dtos
{
    public class TempDetails
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public bool IsProductExist { get; set; }
    }
}
