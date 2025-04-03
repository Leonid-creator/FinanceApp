
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
