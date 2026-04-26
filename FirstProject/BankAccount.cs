using System;
using System.Collections.Generic;

namespace FirstProject
{
    internal class BankAccount
    {
        private List<Transaction> transactions;
        public string OwnerName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }


        public IReadOnlyList<Transaction> Transactions
        {
            get { return transactions; }
        }

        public BankAccount(string ownerName)
        {
            OwnerName = ownerName;
            AccountNumber = GenerateAccountNumber();
            Balance = 0;
            transactions = new List<Transaction>();
        }

        public OperationResult Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Transaction transaction = new Transaction(TransactionType.Deposit, amount);
                transactions.Add(transaction);

                return new OperationResult(true, "Deposit successful.");
            }
            else
            {
                return new OperationResult(false, "Amount must be greater than 0.");
            }
        }

        public OperationResult Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                return new OperationResult(false, "Amount must be greater than 0.");
            }
            else if (amount > Balance)
            {
                return new OperationResult(false, "Insufficient funds");
            }
            else
            {
                Balance -= amount;
                Transaction transaction = new Transaction(TransactionType.Withdraw, amount);
                transactions.Add(transaction);
                return new OperationResult(true, "Withdraw successful");
            }
        }

        public void ShowTransactionHistory()
        {
            if (transactions.Count == 0)
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

        public OperationResult TransferTo(BankAccount receiver, decimal amount)
        {
            if (receiver == null)
            {
                return new OperationResult(false, "Receiver account cannot be null.");
            }
            if (receiver == this)
            {
                return new OperationResult(false, "Cannot transfer to the same account.");
            }

            if (amount == 0)
            {
                return new OperationResult(false, "Amount must be greater than 0.");
            }
            if (amount > Balance)
            {
                return new OperationResult(false, "Insufficient funds.");
            }

            Balance -= amount;
            receiver.Balance += amount;
            Transaction transaction = new Transaction(TransactionType.Transfer, amount);
            transactions.Add(transaction);
            Transaction receiverTransaction = new Transaction(TransactionType.Transfer, amount);
            receiver.transactions.Add(receiverTransaction);
            return new OperationResult(true, "Transfer successful.");

        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}
