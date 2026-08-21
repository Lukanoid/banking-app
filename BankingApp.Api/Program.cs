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

            if (!builder.Environment.IsEnvironment("Testing"))
            {
                string connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
                ?? throw new InvalidOperationException("Postgres connection string is missing.");

                builder.Services.AddDbContext<BankDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });

                builder.Services.AddScoped<IBankStorage, SqliteBankStorage>();
            }

            var app = builder.Build();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                using IServiceScope scope = app.Services.CreateScope();

                BankDbContext context = scope.ServiceProvider.GetRequiredService<BankDbContext>();
                context.Database.Migrate();

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
