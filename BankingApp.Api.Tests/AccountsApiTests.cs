using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using BankingApp.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankingApp.Api.Tests
{
    public class AccountsApiTests
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private static async Task<T> ReadResponseAsync<T>(HttpResponseMessage responses)
        {
            string json = await responses.Content.ReadAsStringAsync();

            T? result = JsonSerializer.Deserialize<T>(json, JsonOptions);

            if(result == null)
            {
                throw new InvalidOperationException("Could not deserialize response.");
            }

            return result;
        }

        private static async Task<AccountResponse> CreateAccountAsync(HttpClient client, string ownerName)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts", new CreateAccountRequest
            {
                OwnerName = ownerName
            });

            response.EnsureSuccessStatusCode();

            return await ReadResponseAsync<AccountResponse>(response);
        }

        [Fact]
        public async Task GetRoot_ShouldReturnRunningMessage()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/");

            string content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Banking API is running", content);
        }

        [Fact]
        public async Task PostAccounts_ShouldCreateAccount_WhenOwnerNameIsValid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts", new CreateAccountRequest
            {
                OwnerName = "John Doe"
            });

            AccountResponse account = await ReadResponseAsync<AccountResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("John Doe", account.OwnerName);
            Assert.False(string.IsNullOrWhiteSpace(account.AccountNumber));
            Assert.Equal(0m, account.Balance);
        }

        [Fact]
        public async Task PostAccounts_ShouldReturnBadRequest_WhenOwnerNameIsInvalid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts", new CreateAccountRequest
            {
                OwnerName = ""
            });

            string message = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Owner name", message);
        }

        [Fact]
        public async Task GetAccounts_ShouldReturnCreatedAccounts()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse firstAccount = await CreateAccountAsync(client, "John Doe");
            AccountResponse secondAcount = await CreateAccountAsync(client, "Vasil");

            HttpResponseMessage response = await client.GetAsync("/accounts");

            List<AccountResponse> accounts = await ReadResponseAsync<List<AccountResponse>>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, accounts.Count);
            Assert.Contains(accounts, account => account.AccountNumber == firstAccount.AccountNumber);
            Assert.Contains(accounts, account => account.AccountNumber == secondAcount.AccountNumber);
        }

        [Fact]
        public async Task GetAccountByNumber_ShouldReturnAccount_WhenAccountExists()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse createdAccount = await CreateAccountAsync(client, "John Doe");

            HttpResponseMessage response = await client.GetAsync($"/accounts/{createdAccount.AccountNumber}");

            AccountResponse account = await ReadResponseAsync<AccountResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(createdAccount.AccountNumber, account.AccountNumber);
            Assert.Equal("John Doe", account.OwnerName);
        }

        [Fact]
        public async Task GetAccountByNumber_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/accounts/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Deposit_ShouldIncreaseBalance_WhenAmountIsValid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            OperationResponse operation = await ReadResponseAsync<OperationResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Deposit successful.", operation.Message);
            Assert.Equal(1000m, operation.Balance);
        }

        [Fact]
        public async Task Deposit_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = -1000m
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            string? message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal("Amount must be greater than 0.", message);
        }

        [Fact]
        public async Task Withdraw_ShouldDecreaseBalance_WhenAmountIsValid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/withdraw", new MoneyRequest
            {
                Amount = 100m
            });

            OperationResponse operation = await ReadResponseAsync<OperationResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Withdraw successful.", operation.Message);
            Assert.Equal(900m, operation.Balance);
        }

        [Fact]
        public async Task Withdraw_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/withdraw", new MoneyRequest
            {
                Amount = -100m
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            string? message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal("Amount must be greater than 0.", message);
        }

        [Fact]
        public async Task Transfer_ShouldMoveMoneyBetweenAccounts_WhenDataIsValid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse sender = await CreateAccountAsync(client, "John Doe");
            AccountResponse receiver = await CreateAccountAsync(client, "Vasil");

            await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/transfer", new TransferRequest
            {
                ReceiverAccountNumber = receiver.AccountNumber,
                Amount = 100m
            });

            TransferResponse transfer = await ReadResponseAsync<TransferResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Transfer successful.", transfer.Message);
            Assert.Equal(900m, transfer.SenderBalance);
            Assert.Equal(100m, transfer.ReceiverBalance);
        }

        [Fact]
        public async Task GetTransactions_ShouldReturnTransactionHistory_WhenTransacationsExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            await client.PostAsJsonAsync($"/accounts/{account.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            HttpResponseMessage response = await client.GetAsync($"/accounts/{account.AccountNumber}/transactions");

            List<TransactionResponse> transactions = await ReadResponseAsync<List<TransactionResponse>>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            TransactionResponse transaction = Assert.Single(transactions);
            Assert.Equal("Deposit", transaction.Type);
            Assert.Equal(1000m, transaction.Amount);
            Assert.False(string.IsNullOrWhiteSpace(transaction.Date));
        }
    }
}
