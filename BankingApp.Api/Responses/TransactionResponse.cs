namespace BankingApp.Api.Responses
{
    public class TransactionResponse
    {
        public string Type { get; set; } = string.Empty; 
        public decimal Amount { get; set; }
        public string Date { get; set; } = string.Empty;

    }
}
