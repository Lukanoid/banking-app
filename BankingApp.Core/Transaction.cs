using System;

namespace BankingApp.Core
{
    public class Transaction
    {
        public TransactionType Type { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }

        public Transaction(TransactionType type, decimal amount)
        {
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }
    }
}
