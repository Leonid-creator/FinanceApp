
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
