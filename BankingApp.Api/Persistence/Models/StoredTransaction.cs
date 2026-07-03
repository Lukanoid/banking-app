using BankingApp.Core;

namespace BankingApp.Api.Persistence.Models
{
    internal class StoredTransaction
    {
        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
