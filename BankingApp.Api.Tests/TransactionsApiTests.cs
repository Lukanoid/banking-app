using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static BankingApp.Api.Tests.ApiTestHelpers;

namespace BankingApp.Api.Tests
{
    public class TransactionsApiTests
    {
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
