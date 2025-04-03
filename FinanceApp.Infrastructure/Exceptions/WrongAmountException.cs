
namespace FinanceApp.Infrastructure.Exceptions
{
    public class WrongAmountException : Exception
    {
        public WrongAmountException() { }
        public WrongAmountException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
