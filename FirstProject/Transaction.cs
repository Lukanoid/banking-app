using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject
{
    internal class Transaction
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
