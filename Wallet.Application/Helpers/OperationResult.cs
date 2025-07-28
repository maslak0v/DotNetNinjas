using System.Text;

namespace Wallet.Application.Helpers
{
    public class OperationResult
    {
        public bool IsSuccess {  get; init; }
        public string? Message { get; init; }
        public Enum_StatusCode StatusCode { get;init; }

        protected OperationResult(bool isSuccess, Enum_StatusCode statusCode, string? message)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public static OperationResult Success(Enum_StatusCode statusCode, string? message = null)
            => new OperationResult(true, statusCode, message);

        public static OperationResult Failure(Enum_StatusCode statusCode, string message)
            => new OperationResult(false, statusCode, message);
        public static OperationResult FromException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            for (var current = ex; current is not null; current = current.InnerException)
                sb.AppendLine(current.Message);
            return Failure(Enum_StatusCode.ServerError, sb.ToString());
        }
    }
}
