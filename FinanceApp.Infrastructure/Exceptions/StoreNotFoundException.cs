
namespace FinanceApp.Infrastructure.Exceptions
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException() {}
        public StoreNotFoundException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
