using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Infrastructure.Exceptions
{
    public class DuplicateReceiptException : Exception
    {
        public DuplicateReceiptException() { }
        public DuplicateReceiptException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
