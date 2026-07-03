using System;

namespace BankingApp.Core
{
    public class Transaction
    {
        public TransactionType Type { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }

        public Transaction(TransactionType type, decimal amount) : this(type, amount, DateTime.Now)
        {

        }

        public Transaction(TransactionType type, decimal amount, DateTime date)
        {
            Type = type;
            Amount = amount;
            Date = date;
        }
    }
}
