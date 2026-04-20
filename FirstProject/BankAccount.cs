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
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        public BankAccount(string ownerName)
        {
            OwnerName = ownerName;
            AccountNumber = GenerateAccountNumber();
            Balance = 0;
            Transactions = new List<Transaction>();
        }

        public OperationResult Deposit(decimal amount)
        {
            if(amount > 0)
            {
                Balance += amount;
                Transaction transaction = new Transaction(TransactionType.Deposit, amount);
                Transactions.Add(transaction);

                return new OperationResult(true, "Deposit successful.");
            }
            else
            {
                return new OperationResult(false, "Amount must be greater than 0.");
            }
        }

        public OperationResult Withdraw(decimal amount)
        {
            if(amount <= 0)
            {
                return new OperationResult(false, "Amount must be greater than 0.");
            }
            else if(amount > Balance)
            {
                return new OperationResult(false, "Insufficient funds");
            }
            else
            {
                Balance -= amount;
                Transaction transaction = new Transaction(TransactionType.Withdraw, amount);
                Transactions.Add(transaction);
                return new OperationResult(true, "Withdraw successful");
            }
        }

        public void ShowTransactionHistory()
        {
            if(Transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            Console.WriteLine("Transaction History:");
            foreach (Transaction transaction in Transactions)
            {
                Console.WriteLine($"{transaction.Type} - {transaction.Amount:F2} - {transaction.Date:dd/MM/yyyy HH:mm}");
            }
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}
