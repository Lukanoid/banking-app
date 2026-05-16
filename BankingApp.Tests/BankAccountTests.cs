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
    }
}
