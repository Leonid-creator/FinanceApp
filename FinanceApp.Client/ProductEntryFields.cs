using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Client
{
    public class ProductEntryFields
    {
        public Entry Quantity { get; set; }
        public Entry ProductName { get; set; }
        public Entry Amount { get; set; }
        public Entry Discount { get; set; }
        public Entry Category { get; set; }
        public Entry Subcategory { get; set; }
    }
}
