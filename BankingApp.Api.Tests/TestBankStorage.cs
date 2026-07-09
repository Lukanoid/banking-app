using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Api.Persistence;
using BankingApp.Core;

namespace BankingApp.Api.Tests
{
    public class TestBankStorage : IBankStorage
    {
        public List<BankAccount> SavedAccounts { get; set; } = new List<BankAccount>();

        public List<BankAccount> LoadAccounts()
        {
            return new List<BankAccount>();
        }

        public void SaveAccounts(IReadOnlyList<BankAccount> accounts)
        {
            SavedAccounts = accounts.ToList();
        }
    }
}
