using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject
{
    internal class BankSystem
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

        public void ShowAllAccounts()
        {
            if (Accounts.Count == 0)
            {
                Console.WriteLine("No accounts to show.");
                return;
            }

            foreach (BankAccount account in Accounts)
            {
                Console.WriteLine($"{account.OwnerName} - {account.AccountNumber}");
            }
        }
    }
}
