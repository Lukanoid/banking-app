namespace BankingApp.Api.Persistence.Models
{
    internal class StoredBankAccount
    {
        public string OwnerName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public List<StoredTransaction> Transactions { get; set; } = new List<StoredTransaction>();
    }
}
