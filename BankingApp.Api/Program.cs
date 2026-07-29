using BankingApp.Core;
using BankingApp.Api.Persistence;
using BankingApp.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSingleton<BankSystem>();

            string DataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");
            Directory.CreateDirectory(DataDirectory);

            string databasePath = Path.Combine(DataDirectory, "banking.db");

            builder.Services.AddDbContext<BankDbContext>(options =>
            {
                options.UseSqlite($"data Source={databasePath}");
            });

            builder.Services.AddScoped<IBankStorage, SqliteBankStorage>();

            var app = builder.Build();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                using IServiceScope scope = app.Services.CreateScope();

                BankDbContext context = scope.ServiceProvider.GetRequiredService<BankDbContext>();
                context.Database.EnsureCreated();

                BankSystem bankSystem = scope.ServiceProvider.GetRequiredService<BankSystem>();
                IBankStorage storage = scope.ServiceProvider.GetRequiredService<IBankStorage>();

                List<BankAccount> savedAccounts = storage.LoadAccounts();
                bankSystem.LoadAccounts(savedAccounts);
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Banking API is running");

            app.MapAccountEndpoints();

            app.Run();
        }
    }
}
