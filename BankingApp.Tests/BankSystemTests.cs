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

        [Fact]
        public void CreateAccount_ShouldThrowErrorWhenInvalidName()
        {
            BankSystem bankSystem = new BankSystem();

            ArgumentException whiteSpaceException =  Assert.Throws<ArgumentException>(() => bankSystem.CreateAccount(" "));
            ArgumentException emptyNameException = Assert.Throws<ArgumentException>(() => bankSystem.CreateAccount(""));

            Assert.Empty(bankSystem.GetAllAccounts());
            Assert.Equal("Owner name cannot be empty.", whiteSpaceException.Message);
            Assert.Equal("Owner name cannot be empty.", emptyNameException.Message);
        }

        [Fact]
        public void CreateAccount_ShouldCreateUniqueAccountNumbers()
        {
            BankSystem banksystem = new BankSystem();

            BankAccount firstAccount = banksystem.CreateAccount("John Doe");
            BankAccount secondAccount = banksystem.CreateAccount("John Doe");

            Assert.NotEqual(firstAccount.AccountNumber, secondAccount.AccountNumber);
        }

        [Fact]
        public void GetAllAccounts_ShouldReturnEmptyList_WhenNoAccountsExist()
        {
            BankSystem banksystem = new BankSystem();

            IReadOnlyList<BankAccount> accounts = banksystem.GetAllAccounts();

            Assert.Empty(accounts);
        }

        [Fact]
        public void FindAccount_ShouldReturnReferenceToTheRightAccount()
        {
            BankSystem banksystem = new BankSystem();

            BankAccount accountToFind = banksystem.CreateAccount("John Doe");
            BankAccount foundAccount = banksystem.FindAccount(accountToFind.AccountNumber);

            Assert.NotNull(foundAccount);
            Assert.Same(accountToFind, foundAccount);
        }
        
        [Fact]
        public void FindAccount_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            BankSystem banksystem = new BankSystem();

            BankAccount nullAcount = banksystem.FindAccount("123");

            Assert.Null(nullAcount);
        }
    }
}