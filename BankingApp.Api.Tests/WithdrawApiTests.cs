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
    public class WithdrawApiTests
    {

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
        public async Task Withdraw_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts/123/withdraw", new MoneyRequest
            {
                Amount = 1000m
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
