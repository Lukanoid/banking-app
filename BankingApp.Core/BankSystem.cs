using System;
using System.Collections.Generic;

namespace BankingApp.Core
{
    public class BankSystem
    {
        private List<BankAccount> Accounts { get; set; }

        public BankSystem()
        {
            Accounts = new List<BankAccount>();
        }

        public BankAccount CreateAccount(string ownerName)
        {
            string accountNumber = GenerateAccountNumber();
            BankAccount newAccount = new BankAccount(ownerName, accountNumber);
            Accounts.Add(newAccount);
            return newAccount;
        }

        public BankAccount FindAccount(string accountNumber)
        {
            foreach(BankAccount account in Accounts)
            {
                if(account.AccountNumber == accountNumber)
                {
                    return account;
                }
            }

            return null;
        }

        public IReadOnlyList<BankAccount> GetAllAccounts()
        {
            return Accounts.AsReadOnly();
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountNumber = random.Next(100, 999).ToString();

            if(FindAccount(accountNumber) != null)
            {
                return GenerateAccountNumber();
            }

            return accountNumber;
        }
    }
}
