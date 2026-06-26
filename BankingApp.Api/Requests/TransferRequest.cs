namespace BankingApp.Api.Requests
{
    public class TransferRequest
    {
        public string ReceiverAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
