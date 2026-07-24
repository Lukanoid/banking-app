namespace BankingApp.Api.Responses
{
    public class AccountResponse
    {
        public string OwnerName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty; 
        public decimal Balance { get; set; } 

    }
}
