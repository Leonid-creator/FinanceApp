
namespace FinanceApp.Infrastructure.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() { }
        public CategoryNotFoundException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
