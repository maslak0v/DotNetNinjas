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
        public static void HandleWarningError<T>(ILogger<T> logger, string error) where T : class
        {
            logger.LogWarning(error);
            throw new Exception(error);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <param name="error"></param>
        /// <exception cref="Exception"></exception>
        public static void HandleFatalError<T>(ILogger<T> logger, string error) where T : class
        {
            logger.LogError(error);
            throw new Exception(error);
        }
    }
}
