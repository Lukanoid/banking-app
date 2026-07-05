using BankingApp.Api.Persistence;
using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using BankingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BankingApp.Api.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this WebApplication app)
        {
            RouteGroupBuilder accounts = app.MapGroup("/accounts")
                .WithTags("Accounts");

            accounts.MapGet("", (BankSystem bankSystem) =>
            {
                List<AccountResponse> accounts = bankSystem.GetAllAccounts()
                    .Select(account => new AccountResponse
                    {
                        OwnerName = account.OwnerName,
                        AccountNumber = account.AccountNumber,
                        Balance = account.Balance
                    })
                    .ToList();

                return Results.Ok(accounts);
            });

            accounts.MapGet("/{accountNumber}", (string accountNumber, BankSystem bankSystem) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }

                AccountResponse response = new AccountResponse
                {
                    OwnerName = account.OwnerName,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance
                };

                return Results.Ok(response);
            });

            accounts.MapPost("", (CreateAccountRequest request, BankSystem bankSystem, IBankStorage storage) =>
            {
                try
                {
                    BankAccount account = bankSystem.CreateAccount(request.OwnerName);

                    storage.SaveAccounts(bankSystem.GetAllAccounts());

                    AccountResponse response = new AccountResponse
                    {
                        OwnerName = account.OwnerName,
                        AccountNumber = account.AccountNumber,
                        Balance = account.Balance
                    };

                    return Results.Ok(response);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            accounts.MapPost("/{accountNumber}/deposit", (string accountNumber, MoneyRequest request, BankSystem bankSystem, IBankStorage storage) =>
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

                storage.SaveAccounts(bankSystem.GetAllAccounts());

                OperationResponse response = new OperationResponse
                {
                    Message = result.Message,
                    Balance = account.Balance
                };

                return Results.Ok(response);
            });

            accounts.MapPost("/{accountNumber}/withdraw", (string accountNumber, MoneyRequest request, BankSystem bankSystem, IBankStorage storage) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }

                OperationResult result = account.Withdraw(request.Amount);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Message);
                }

                storage.SaveAccounts(bankSystem.GetAllAccounts());

                OperationResponse response = new OperationResponse
                {
                    Message = result.Message,
                    Balance = account.Balance
                };

                return Results.Ok(response);
            });

            accounts.MapPost("/{accountNumber}/transfer", (string accountNumber, TransferRequest request, BankSystem bankSystem, IBankStorage storage) =>
            {
                BankAccount sender = bankSystem.FindAccount(accountNumber);

                if (sender == null)
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

                storage.SaveAccounts(bankSystem.GetAllAccounts());

                TransferResponse response = new TransferResponse
                {
                    Message = result.Message,
                    SenderBalance = sender.Balance,
                    ReceiverBalance = receiver.Balance
                };

                return Results.Ok(response);
            });

            accounts.MapGet("/{accountNumber}/transactions", (string accountNumber, BankSystem bankSystem) =>
            {
                BankAccount account = bankSystem.FindAccount(accountNumber);

                if (account == null)
                {
                    return Results.NotFound("Account not found.");
                }

                List<TransactionResponse> transactions = account.GetTransactionHistory()
                    .Select(transaction => new TransactionResponse
                    {
                        Type = transaction.Type.ToString(),
                        Amount = transaction.Amount,
                        Date = transaction.Date.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                    .ToList();

                return Results.Ok(transactions);
            });
        }
    }
}