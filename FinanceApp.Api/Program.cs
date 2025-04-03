using Microsoft.EntityFrameworkCore;
using FinanceApp.Lib.Dtos;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Repositories;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Infrastructure;
using FinanceApp.Infrastructure.Repositories.Interfaces;

namespace FinanceApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<FinanceAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                ));

            builder.Services.AddScoped<FinanceRepository>();
            builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
            builder.Services.AddScoped<IPurchaseDetailRepository, PurchaseDetailRepository>();
            builder.Services.AddScoped(typeof(IGeneralFinanceRepository<>), typeof(GeneralFinanceRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<TempReceipt>();
            builder.Services.AddScoped<List<TempDetails>>();
            builder.Services.AddScoped<ReceiptProcessor>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
