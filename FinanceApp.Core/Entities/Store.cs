namespace FinanceApp.Core.Entities
{
    public class Store
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
