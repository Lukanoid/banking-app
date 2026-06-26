using BankingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using BankingApp.Api.Requests;

namespace BankingApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

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

            app.MapGet("/accounts/{accountNumber}", (string accountNumber) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }
                return Results.Ok(new
                {
                    account.OwnerName,
                    account.AccountNumber,
                    account.Balance
                });
            });

            app.MapPost("/accounts/{accountNumber}/deposit", (string accountNumber, MoneyRequest request) =>
            {
            BankAccount account = bankSystem.FindAccount(accountNumber);

            if (account == null)
            {
                return Results.NotFound("Account not found.");
            }

            OperationResult result = account.Deposit(request.Amount);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(new 
            {
                result.Message,
                account.Balance
            });
            });

            app.MapPost("accounts/{accountNumber}/withdraw", (string accountNumber, MoneyRequest request) => 
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);
                
                if(account == null)
                {
                    return Results.NotFound("Account not found.");
                }

                OperationResult result = account.Withdraw(request.Amount);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Message);
                }

                return Results.Ok(new
                {
                    result.Message,
                    account.Balance
                });
            });

            app.Run();
        }
    }
}
