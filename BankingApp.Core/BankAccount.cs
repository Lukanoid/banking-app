using System;
using System.Collections.Generic;

namespace BankingApp.Core
{
    public class BankAccount
    {
        private List<Transaction> transactions;
        public string OwnerName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }


        public BankAccount(string ownerName, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                throw new ArgumentException("Owner name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number cannot be empty.");
            }
            OwnerName = ownerName.Trim();
            AccountNumber = accountNumber;
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
                return new OperationResult(false, "Insufficient funds.");
            }
            else
            {
                Balance -= amount;
                Transaction transaction = new Transaction(TransactionType.Withdraw, amount);
                transactions.Add(transaction);
                return new OperationResult(true, "Withdraw successful.");
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

            if (amount <= 0)
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

        public IReadOnlyList<Transaction> GetTransactionHistory()
        {
            return transactions.AsReadOnly();
        }


    }
}
