using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject
{
    internal class BankAccount
    {
        public string OwnerName { get; private set; }
        public decimal Balance { get; private set; }

        public List<Transaction> Transactions { get; private set; }

        public BankAccount(string ownerName)
        {
            OwnerName = ownerName;
            Balance = 0;
            Transactions = new List<Transaction>();
        }

        public void Deposit(decimal amount)
        {
            if(amount > 0)
            {
                Balance += amount;
                Transaction transaction = new Transaction();
                transaction.Type = "Deposit";
                transaction.Amount = amount;
                transaction.Date = DateTime.Now;

                Transactions.Add(transaction);

                Console.WriteLine("Deposit successful.");
            }
            else
            {
                Console.WriteLine("Amount must be greater than 0.");
            }
        }

        public void Withdraw(decimal amount)
        {
            if(amount <= 0)
            {
                Console.WriteLine("Amount must be greater than 0");
            }
            else if(amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
            }
            else
            {
                Balance -= amount;
                Transaction transaction = new Transaction();
                transaction.Type = "Withdrawal";
                transaction.Amount = amount;
                transaction.Date = DateTime.Now;
                Transactions.Add(transaction);
                Console.WriteLine("Withdrawal successful.");
            }
        }

        public void ShowTransactions()
        {
            if(Transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach(Transaction transaction in Transactions)
            {
                Console.WriteLine($"{transaction.Type} - {transaction.Amount} - {transaction.Date}");
            }
        }

    }
}
