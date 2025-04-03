
namespace FinanceApp.Lib.Dtos
{
    public class TempReceipt
    {
        public string StoreName { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceiptDiscount { get; set; }
    }
}
