using Xunit;
using BankingApp.Core;

namespace BankingApp.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void Constructor_ShouldCreateTransaction_WhenDataIsValid()
        {
            Transaction transaction = new Transaction(TransactionType.Deposit, 100m);

            Assert.Equal(TransactionType.Deposit, transaction.Type);
            Assert.Equal(100m, transaction.Amount);
            Assert.True(transaction.Date <= DateTime.Now);
        }
    }
}
