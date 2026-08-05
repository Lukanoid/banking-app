using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using System.Net;
using System.Net.Http.Json;
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

        [Fact]
        public async Task GetTransaction_ShouldReturnTransferDescriptions_WhenTransferExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse sender = await CreateAccountAsync(client, "John Doe");
            AccountResponse receiver = await CreateAccountAsync(client, "Vasil");

            await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/deposit", new MoneyRequest
            {
                Amount = 1000m
            });

            await client.PostAsJsonAsync($"/accounts/{sender.AccountNumber}/transfer", new TransferRequest
            {
                ReceiverAccountNumber = receiver.AccountNumber,
                Amount = 100m
            });

            HttpResponseMessage senderResponse = await client.GetAsync($"/accounts/{sender.AccountNumber}/transactions");
            HttpResponseMessage receiverResponse = await client.GetAsync($"/accounts/{receiver.AccountNumber}/transactions");

            Assert.Equal(HttpStatusCode.OK, senderResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, receiverResponse.StatusCode);

            List<TransactionResponse> senderTransactions = await ReadResponseAsync<List<TransactionResponse>>(senderResponse);
            List<TransactionResponse> receiverTransactions = await ReadResponseAsync<List<TransactionResponse>>(receiverResponse);

            Assert.Contains(senderTransactions, transaction =>
                transaction.Type == "TransferOut" &&
                transaction.Amount == 100m &&
                transaction.Description == $"Transfer to {receiver.AccountNumber}");

            Assert.Contains(receiverTransactions, transaction =>
                transaction.Type == "TransferIn" &&
                transaction.Amount == 100m &&
                transaction.Description == $"Transfer from {sender.AccountNumber}");
        }

        [Fact]
        public async Task GetTransactions_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/accounts/99999/transactions");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
