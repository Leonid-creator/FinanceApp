namespace FinanceApp.Infrastructure.Repositories.Interfaces
{
    public interface IGeneralFinanceRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> AddEntityAsync(T entity);
        Task<T?> GetByIdAsync(int entityId);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int entityId);
    }
}