namespace Akkad.Commands
{
    public class AsyncTaskResult
    {
        public static readonly AsyncTaskResult Success = new AsyncTaskResult(AsyncTaskStatus.Success, null);

        public AsyncTaskStatus Status { get; private set; }

        public string ErrorMessage { get; private set; }

        public AsyncTaskResult(AsyncTaskStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
    }

    public enum AsyncTaskStatus
    {
        Success,
        IoException,
        Failed
    }
}