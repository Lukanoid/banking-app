namespace BankingApp.Api.Requests
{
    public class TransferRequest
    {
        public string ReceiverAccountNumber { get; set; }  = string.Empty;

        public decimal Amount { get; set; }
    }
}
