using System;

namespace BankingApp.Core
{
    public class Transaction
    {
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }

        public string Description { get; private set; }

        public Transaction(TransactionType type, decimal amount)
            :this(type, amount, DateTime.Now, string.Empty)
        {

        }

        public Transaction(TransactionType type, decimal amount, string description)
            : this(type, amount, DateTime.Now, description)
        {

        }

        public Transaction(TransactionType type, decimal amount, DateTime date) 
            : this(type, amount, date, string.Empty)
        {

        }

        public Transaction(TransactionType type, decimal amount, DateTime date, string description)
        {
            Type = type;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}
