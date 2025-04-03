
namespace FinanceApp.Infrastructure.Exceptions
{
    public class CategoryAlreadyExistsException : Exception
    {
        public CategoryAlreadyExistsException() { }
        public CategoryAlreadyExistsException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
