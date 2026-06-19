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
        public void Deposit_ShouldAddMoneyToBalance_WhenAmountIsPositive()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            OperationResult result = bankAccount.Deposit(100m);

            Assert.Equal(100m, bankAccount.Balance);
            Assert.True(result.IsSuccess);
            Assert.Equal("Deposit successful.", result.Message);
            Assert.Single(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Deposit_ShouldNotAddMoneyToTheBalance_WhenDataIsZero()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            OperationResult result = bankAccount.Deposit(0m);

            Assert.False(result.IsSuccess);
            Assert.Equal(0m, bankAccount.Balance);
            Assert.Equal("Amount must be greater than 0.", result.Message);
            Assert.Empty(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Deposit_ShouldNotAddMoneyToTheBalance_WhenDataIsBelowZero()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            OperationResult result = bankAccount.Deposit(-100m);

            Assert.False(result.IsSuccess);
            Assert.Equal(0, bankAccount.Balance);
            Assert.Equal("Amount must be greater than 0.", result.Message);
            Assert.Empty(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Withdraw_ShouldRemoveMoneyFromTheBalance_WhenAmoutIsValid()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(1000m);

            OperationResult result = bankAccount.Withdraw(100m);
            IReadOnlyList<Transaction> transactions = bankAccount.GetTransactionHistory();

            Assert.True(result.IsSuccess);
            Assert.Equal(900m, bankAccount.Balance);
            Assert.Equal("Withdraw successful.", result.Message);
            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        public void Withdraw_ShouldFailAndNotChangeBalance_WhenAmountIsZero()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(1000m);

            OperationResult result = bankAccount.Withdraw(0m);

            Assert.False(result.IsSuccess);
            Assert.Equal(1000m, bankAccount.Balance);
            Assert.Equal("Amount must be greater than 0.", result.Message);
            Assert.Single(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Withdraw_ShouldFailAndNotChangeBalance_WhenAmountIsBelowZero()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(1000m);

            OperationResult result = bankAccount.Withdraw(-100m);

            Assert.False(result.IsSuccess);
            Assert.Equal(1000m, bankAccount.Balance);
            Assert.Equal("Amount must be greater than 0.", result.Message);
            Assert.Single(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Withdraw_ShouldFailAndNotChangeBalance_WhenAmountIsMoreThanBalance()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(1000m);

            OperationResult result = bankAccount.Withdraw(2000m);

            Assert.False(result.IsSuccess);
            Assert.Equal(1000m, bankAccount.Balance);
            Assert.Equal("Insufficient funds", result.Message);
            Assert.Single(bankAccount.GetTransactionHistory());
        }

        [Fact]
        public void Withdraw_ShouldSetBalanceToZero_WhenAmountIsEqualToBalance()
        {
            BankAccount bankAccount = new BankAccount("John Doe", "123");

            bankAccount.Deposit(1000m);

            OperationResult result = bankAccount.Withdraw(1000m);
            IReadOnlyList<Transaction> transactions = bankAccount.GetTransactionHistory();

            Assert.True(result.IsSuccess);
            Assert.Equal(0m, bankAccount.Balance);
            Assert.Equal("Withdraw successful.", result.Message);
            Assert.Equal(2, transactions.Count);
            Assert.Equal(TransactionType.Withdraw, transactions[1].Type);
        }

        [Fact]
        public void TransferTo_ShouldTransferMoneyToAccount_WhenDataIsValid()
        {
            BankAccount transferer = new BankAccount("John Doe", "123");
            BankAccount receiver = new BankAccount("Vasil Stamboliyski", "321");

            transferer.Deposit(1000m);
            OperationResult result = transferer.TransferTo(receiver, 100m);
            IReadOnlyList<Transaction> transfererTransactionHistory = transferer.GetTransactionHistory();
            IReadOnlyList<Transaction> receiverTransactionHistory = receiver.GetTransactionHistory();

            Assert.Equal(900m, transferer.Balance);
            Assert.Equal(100m, receiver.Balance);
            Assert.Equal("Transfer successful.", result.Message);
            Assert.True(result.IsSuccess);
            Assert.Single(receiverTransactionHistory);
            Assert.Equal(2, transfererTransactionHistory.Count);
            Assert.Equal(TransactionType.Transfer, transfererTransactionHistory[1].Type);
            Assert.Equal(TransactionType.Transfer, receiverTransactionHistory[0].Type);
        }
    }
}
