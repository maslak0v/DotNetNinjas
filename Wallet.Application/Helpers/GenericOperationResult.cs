using System.Text;

namespace Wallet.Application.Helpers;

public class OperationResult<T> : OperationResult
{
    public T? Result { get; init; }

    private OperationResult(bool isSuccess, Enum_StatusCode statusCode, string? message, T? result)
        : base(isSuccess, statusCode, message)
    {
        Result = result;
    }

    public static OperationResult<T> Success(Enum_StatusCode statusCode, T result, string? message = null)
        => new OperationResult<T>(true, statusCode, message, result);

    public new static OperationResult<T> Failure(Enum_StatusCode statusCode, string message)
        => new OperationResult<T>(false, statusCode, message, default);

    public new static OperationResult<T> FromException(Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        for (var current = ex; current is not null; current = current.InnerException)
            sb.AppendLine(current.Message);
        return new OperationResult<T>(false, Enum_StatusCode.ServerError, sb.ToString(), default);
    }
}