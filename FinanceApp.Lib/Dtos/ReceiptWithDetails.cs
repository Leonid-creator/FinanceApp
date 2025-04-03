
namespace FinanceApp.Lib.Dtos
{
    public class ReceiptWithDetails
    {
        public TempReceipt tempReceipt { get; set; }
        public List<TempDetails> tempDetails { get; set; }
    }
}
