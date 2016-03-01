using Akkad.Commands;

namespace Akkad.Resources
{
    public static class AsyncTaskResultExtensions
    {
        public static bool IsSuccess(this AsyncTaskResult result)
        {
            if (result.Status != AsyncTaskStatus.Success)
            {
                return false;
            }
            return true;
        }

        public static string GetErrorMessage(this AsyncTaskResult result)
        {
            if (result.Status != AsyncTaskStatus.Success)
            {
                return result.ErrorMessage;
            }
            return null;
        }
    }
}