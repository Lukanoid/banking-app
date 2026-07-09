using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using System.Net;

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

            HttpResponseMessage response = await client.GetAsync("/account/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
