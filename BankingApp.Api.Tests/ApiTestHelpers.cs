using System.Net.Http.Json;
using BankingApp.Api.Requests;
using BankingApp.Api.Responses;

namespace BankingApp.Api.Tests
{
    public static class ApiTestHelpers
    {
        public static async Task<T> ReadResponseAsync<T>(HttpResponseMessage response)
        {
            T? result = await response.Content.ReadFromJsonAsync<T>();

            if (result == null)
            {
                throw new InvalidOperationException("Could not deserialize response.");
            }

            return result;
        }

        public static async Task<AccountResponse> CreateAccountAsync(HttpClient client, string ownerName)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/accounts", new CreateAccountRequest
            {
                OwnerName = ownerName
            });

            response.EnsureSuccessStatusCode();

            return await ReadResponseAsync<AccountResponse>(response);
        }

        public static async Task DepositAsync(HttpClient client, string accountNumber, decimal amount)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"/accounts/${accountNumber}/deposit", new MoneyRequest
            {
                Amount = amount
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
