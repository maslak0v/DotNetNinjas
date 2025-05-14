namespace FinancialTracker.Services.AuthorizeApi.Application.Features
{
    public class ErrorHandler
    {
        public static OperationResult HandleWarningError<TLoggingObject>
            (ILogger<TLoggingObject> logger, string error) where TLoggingObject : class
        {
            logger.LogWarning(error);
            return OperationResultCreator.Failure(error);
        }
        public static OperationResult HandleFatalError<TLoggingObject>
            (ILogger<TLoggingObject> logger, string error) where TLoggingObject : class
        {
            logger.LogError(error);
            return OperationResultCreator.Failure(error);
        }

        public static OperationResult<TResult> HandleWarningError<TLoggingObject, TResult>
            (ILogger<TLoggingObject> logger, string error) where TLoggingObject : class
        {
            logger.LogWarning(error);
            return OperationResultCreator.Failure<TResult>(error);
        }

        public static OperationResult<TResult> HandleInformation<TLoggingObject, TResult>
            (ILogger<TLoggingObject> logger, string error) where TLoggingObject : class
        {
            logger.LogInformation(error);
            return OperationResultCreator.Failure<TResult>(error);
        }
    }
}
