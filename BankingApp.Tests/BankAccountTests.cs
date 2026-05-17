using Xunit;
using BankingApp.Core;

namespace BankingApp.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Constructor_ShouldCreateBankAccount_WhenDataIsValid()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");
            IReadOnlyList<Transaction> transactions = bankAccount.GetTransactionHistory();

            Assert.Equal("John Doe", bankAccount.OwnerName);
            Assert.Equal("123", bankAccount.AccountNumber);
            Assert.Equal(0, bankAccount.Balance);
            Assert.Empty(transactions);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenOwnerNameIsInvalid()
        {
            ArgumentException invalidName = Assert.Throws<ArgumentException>(() => new BankAccount("", "123"));
            Assert.Equal("Owner name cannot be empty.", invalidName.Message);

        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenAccountNumberIsInvalid()
        {
            ArgumentException emptyAccountNumber = Assert.Throws<ArgumentException>(() => new BankAccount("John Doe", ""));
            ArgumentException whitespaceAccountNumber = Assert.Throws<ArgumentException>(() => new BankAccount("John Doe", " "));
            ArgumentException nullAccountNumber = Assert.Throws<ArgumentException>(() => new BankAccount("John Doe", null));

            Assert.Equal("Account number cannot be empty.", emptyAccountNumber.Message);
            Assert.Equal("Account number cannot be empty.", whitespaceAccountNumber.Message);
            Assert.Equal("Account number cannot be empty.", nullAccountNumber.Message);
        }

        [Fact]
        public void Constructor_ShouldTrimOwnerName()
        {
            BankAccount bankAccount = new BankAccount(" John Doe ", "123");

            Assert.Equal("John Doe", bankAccount.OwnerName);
        }

        [Fact]
        public void Deposit_ShouldAddMoneyToBalance_WhenDataIsValid()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(100m);

            Assert.Equal(100m, bankAccount.Balance);
        }
    }
}
