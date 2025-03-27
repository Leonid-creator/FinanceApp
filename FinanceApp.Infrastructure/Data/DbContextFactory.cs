using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinanceApp.Infrastructure.Data
{
    public class FinanceAppDbContextFactory : IDesignTimeDbContextFactory<FinanceAppDbContext>
    {
        public FinanceAppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FinanceApp.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FinanceAppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FinanceAppDbContext(optionsBuilder.Options);
        }
    }
}