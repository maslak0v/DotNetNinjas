namespace FinancialTracker.Services.AuthorizeApi.Application.Features
{
    public class ErrorHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <param name="error"></param>
        /// <exception cref="Exception"></exception>
        public static OperationResult HandleWarningError<T>(ILogger<T> logger, string error) where T : class
        {
            logger.LogWarning(error);
            return OperationResultCreator.Failure(error);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <param name="error"></param>
        /// <exception cref="Exception"></exception>
        public static OperationResult HandleFatalError<T>(ILogger<T> logger, string error) where T : class
        {
            logger.LogError(error);
            return OperationResultCreator.Failure(error);
        }
    }
}
