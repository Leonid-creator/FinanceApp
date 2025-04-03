
namespace FinanceApp.Infrastructure.Exceptions
{
    public class SubcategoryNotFoundException : Exception
    {
        public SubcategoryNotFoundException() { }
        public SubcategoryNotFoundException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
