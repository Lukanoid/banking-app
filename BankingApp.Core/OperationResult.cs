namespace BankingApp.Core
{
    public class OperationResult
    {
        public bool IsSuccess { get; }

        public string Message { get; }

        public OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
