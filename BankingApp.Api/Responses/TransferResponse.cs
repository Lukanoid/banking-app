namespace BankingApp.Api.Responses
{
    public class TransferResponse
    {
        public string Message { get; set; }
        public decimal SenderBalance { get; set; }
        public decimal ReceiverBalance { get; set; }

    }
}
