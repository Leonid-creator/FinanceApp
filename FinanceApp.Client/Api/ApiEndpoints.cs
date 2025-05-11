
namespace FinanceApp.Client.Api
{
    public class ApiEndpoints
    {
#if DEBUG
        public const string BaseUrl = "http://localhost:5133/api/";
        //public const string BaseUrl = "https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/";
#else
        public const string BaseUrl = "https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/";
#endif

        public static string GetStores => $"{BaseUrl}stores/get-stores";
        public static string GetSubcategoriesByCategory => $"{BaseUrl}subcategories/get-subcategories-by-category-name";
        public static string AddStore => $"{BaseUrl}stores/add-store";
    }
}
