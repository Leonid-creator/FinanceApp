using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Lib.Dtos
{
    public class ReceiptWithDetails
    {
        public TempReceipt tempReceipt { get; set; }
        public List<TempDetails> tempDetails { get; set; }
    }
}
