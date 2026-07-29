using BankingApp.Core;

namespace BankingApp.Api.Persistence.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }

        public TransactionType type { get; set; }

        public decimal Amount  { get; set; }

        public DateTime Date { get; set; }

        public int BankAccountEntityId { get; set; }

        public BankAccountEntity? Account { get; set; }
    }
}
