using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject
{
    internal class BankAccount
    {
        public string OwnerName { get; set; }
        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; set; }

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

    }
}
