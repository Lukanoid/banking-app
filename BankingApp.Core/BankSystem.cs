using System;
using System.Collections.Generic;

namespace BankingApp.Core
{
    public class BankSystem
    {
        public List<BankAccount> Accounts { get; private set; }

        public BankSystem()
        {
            Accounts = new List<BankAccount>();
        }

        public BankAccount CreateAccount(string ownerName)
        {
            BankAccount newAccount = new BankAccount(ownerName);
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
    }
}
