using BankingApp.Core;

namespace BankingApp.Api.Persistence.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount  { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public int BankAccountEntityId { get; set; }

        public BankAccountEntity? Account { get; set; }
    }
}
