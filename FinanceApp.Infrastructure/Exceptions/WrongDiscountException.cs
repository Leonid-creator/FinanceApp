using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Infrastructure.Exceptions
{
    public class WrongDiscountException : Exception
    {
        public WrongDiscountException() { }
        public WrongDiscountException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
