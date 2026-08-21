using BankingApp.Api.Requests;
using BankingApp.Api.Responses;
using System.Net;
using System.Net.Http.Json;
using static BankingApp.Api.Tests.ApiTestHelpers;

namespace BankingApp.Api.Tests
{
    public class UpdateOwnerNameApiTests
    {
        [Fact]
        public async Task UpdateOwnerName_ShouldUpdateOwnerName_WhenDataIsValid()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"/accounts/{account.AccountNumber}/owner",
                new UpdateOwnerNameRequest
                {
                    OwnerName = "Vasil Stamboliyski"
                });

            AccountResponse updatedAccount = await ReadResponseAsync<AccountResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Vasil Stamboliyski", updatedAccount.OwnerName);
            Assert.Equal(account.AccountNumber, updatedAccount.AccountNumber);
            Assert.Equal(account.Balance, updatedAccount.Balance);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateOwnerName_ShouldReturnBadRequest_WhenOwnerNameIsInvalid(string ownerName)
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            AccountResponse account = await CreateAccountAsync(client, "John Doe");

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"/accounts/{account.AccountNumber}/owner",
                new UpdateOwnerNameRequest
                {
                    OwnerName = ownerName
                });

            string message = await ReadResponseAsync<string>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Owner name cannot be empty.", message);
        }

        [Fact]
        public async Task UpdateOwnerName_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            using HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.PutAsJsonAsync(
                "/accounts/missing-account/owner",
                new UpdateOwnerNameRequest
                {
                    OwnerName = "Vasil Stamboliyski"
                });

            string message = await ReadResponseAsync<string>(response);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Account not found.", message);
        }
    }
}