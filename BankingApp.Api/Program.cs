using BankingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            BankSystem bankSystem = new BankSystem();

            app.MapGet("/", () => "Banking API is running");

            app.MapGet("/accounts", () =>
            {
                return bankSystem.GetAllAccounts();
            });

            app.MapPost("/accounts", (CreateAccountRequest request) =>
            {
                if (string.IsNullOrWhiteSpace(request.OwnerName))
                {
                    return Results.BadRequest("Owner name is required.");
                }

                BankAccount account = bankSystem.CreateAccount(request.OwnerName);

                return Results.Ok(new
                {
                    account.OwnerName,
                    account.AccountNumber,
                    account.Balance
                });
            });

            app.Run();
        }
    }
}
