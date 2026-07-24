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
    }
}
