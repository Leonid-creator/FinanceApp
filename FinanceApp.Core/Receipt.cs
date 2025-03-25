using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Core
{
    public class Receipt
    {
        public int ReceiptID { get; set; }
        public int StoreID { get; set; }
        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceiptDiscount { get; set; }
        public string? Comment { get; set; }

        [ForeignKey("StoreID")]
        public Store Store { get; set; }
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
