using FinanceApp.Core.Entities;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Infrastructure.Repositories
{
    public class ReceiptRepository : GeneralFinanceRepository<Receipt>, IReceiptRepository
    {
        private readonly FinanceAppDbContext _dbContext;
        public ReceiptRepository(FinanceAppDbContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}
