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
    public class TransferToApiTests
    {
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
        public async Task Transfer_ShouldReturnNotFound_WhenSenderDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse receiver = await CreateAccountAsync(client, "Vasil");

            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts/99999/transfer", new TransferRequest
            {
                ReceiverAccountNumber = receiver.AccountNumber,
                Amount = 100m
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            string? message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal("Sender account not found.", message);
        }

        [Fact]
        public async Task Transfer_ShouldReturnNotFound_WhenReceiverAccountDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse sender = await CreateAccountAsync(client, "John Doe");

            await DepositAsync(client, sender.AccountNumber, 1000m);

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/transfer", new TransferRequest
            {
                ReceiverAccountNumber = "missing-receiver",
                Amount = 100m
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            string message = await response.Content.ReadAsStringAsync();

            Assert.Contains("Receiver account not found.", message);

        }

        [Fact]
        public async Task Transfer_ShouldReturnBadRequest_WhenAmountIsZero()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse sender = await CreateAccountAsync(client, "John Doe");
            AccountResponse receiver = await CreateAccountAsync(client, "Vasil");

            await DepositAsync(client, sender.AccountNumber, 1000m);

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/transfer", new TransferRequest
            {
                ReceiverAccountNumber = receiver.AccountNumber,
                Amount = 0m
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            string? message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal("Amount must be greater than 0.", message);
        }

        [Fact]
        public async Task Transfer_ShouldReturnBadRequest_WhenAmountIsNegative()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse sender = await CreateAccountAsync(client, "John Doe");
            AccountResponse receiver = await CreateAccountAsync(client, "Vasil");

            await DepositAsync(client, sender.AccountNumber, 1000m);

            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/transfer", new TransferRequest
            {
                ReceiverAccountNumber = receiver.AccountNumber,
                Amount = -100m
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            string? message = await response.Content.ReadFromJsonAsync<string>();

            Assert.Equal("Amount must be greater than 0.", message);
        }
    }
}
