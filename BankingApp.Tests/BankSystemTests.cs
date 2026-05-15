using BankingApp.Core;
using Xunit;

namespace BankingApp.Tests
{
    public class BankSystemTests
    {
        [Fact]
        public void CreateAccount_ShouldAddAccountToAccountList()
        {
            BankSystem bankSystem = new BankSystem();

            BankAccount account = bankSystem.CreateAccount("John Doe");

            Assert.Single(bankSystem.GetAllAccounts());
            Assert.Equal("John Doe", account.OwnerName);
            Assert.Contains(account, bankSystem.GetAllAccounts());
        }
    }
}