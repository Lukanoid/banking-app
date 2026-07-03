using BankingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using BankingApp.Api.Requests;
using BankingApp.Api.Responses;

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
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Banking API is running");

            app.MapGet("/accounts", (BankSystem bankSystem) =>
            {
                return Results.Ok(bankSystem.GetAllAccounts());
            });

            app.MapPost("/accounts", (CreateAccountRequest request, BankSystem bankSystem) =>
            {
                try
                {
                    BankAccount account = bankSystem.CreateAccount(request.OwnerName);

                    return Results.Ok(new AccountResponse
                    {
                        OwnerName = account.OwnerName,
                        AccountNumber = account.AccountNumber,
                        Balance = account.Balance
                    });
                }
                catch(ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapGet("/accounts/{accountNumber}", (string accountNumber, BankSystem bankSystem) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }
                return Results.Ok(new AccountResponse
                {
                    OwnerName = account.OwnerName,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance
                });
            });

            app.MapPost("/accounts/{accountNumber}/deposit", (string accountNumber, MoneyRequest request, BankSystem bankSystem) =>
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

            return Results.Ok(new OperationResponse
            {
                Message = result.Message,
                Balance = account.Balance
            });
            });

            app.MapPost("/accounts/{accountNumber}/withdraw", (string accountNumber, MoneyRequest request, BankSystem bankSystem) => 
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

                return Results.Ok(new OperationResponse
                {
                    Message = result.Message,
                    Balance = account.Balance
                });
            });

            app.MapPost("/accounts/{accountNumber}/transfer", (string accountNumber, TransferRequest request, BankSystem bankSystem) =>
            {
                BankAccount sender = bankSystem.FindAccount(accountNumber);

                if(sender == null)
                {
                    return Results.NotFound("Sender account not found.");
                }

                BankAccount receiver = bankSystem.FindAccount(request.ReceiverAccountNumber);

                if (receiver == null)
                {
                    return Results.NotFound("Receiver account not found.");
                }

                OperationResult result = sender.TransferTo(receiver, request.Amount);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Message);
                }

                return Results.Ok(new TransferResponse
                {
                    Message = result.Message,
                    SenderBalance = sender.Balance,
                    ReceiverBalance = receiver.Balance
                });
            });

            app.MapGet("/accounts/{accountNumber}/transactions", (string accountNumber, BankSystem bankSystem) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }

                var transactions = account.GetTransactionHistory().Select(transaction => new TransactionResponse
                {
                    Type = transaction.Type.ToString(),
                    Amount = transaction.Amount,
                    Date = transaction.Date.ToString("yyyy-MM-dd HH:mm:ss")
                });

                return Results.Ok(transactions);
            });

            app.Run();
        }
    }
}
