using Xunit;
using BankingApp.Core;


namespace BankingApp.Tests
{
    public class OperationResultTests
    {
        [Fact]
        public void Constructor_ShouldCreateSuccessfulResult_WhenDataIsValid()
        {
            OperationResult result = new OperationResult(true, "Deposit successful.");

            Assert.True(result.IsSuccess);
            Assert.Equal("Deposit successful.", result.Message);
        }

        [Fact]
        public void Constructor_ShouldCreateFailedResult_WhenDataIsValid()
        {
            OperationResult result = new OperationResult(false, "Amount must be greater than 0.");

            Assert.False(result.IsSuccess);
            Assert.Equal("Amount must be greater than 0.", result.Message);
        }
    }
}
