namespace BankingApp.Api.Persistence.Entities
{
    public class BankAccountEntity
    {
        public int Id { get; set; }

        public string OwnerName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public List<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
    }
}
