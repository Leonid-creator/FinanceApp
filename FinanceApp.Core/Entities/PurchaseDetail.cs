using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Core.Entities
{
    public class PurchaseDetail
    {
        public int ReceiptID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public Receipt Receipt { get; set; }
        public Product Product { get; set; }
    }
}
