using FinancialTracker.Services.AuthorizeApi.Application.Extensions;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.Features
{
    public record OperationResult
        (bool IsSuccess, Enum_StatusCode StatusCode, string? Message = null);
    public record OperationResult<TResult>
        (TResult? Result, bool IsSuccess, Enum_StatusCode status, string? Message = null);
    public static class OperationResultCreator
    {
        public static OperationResult Success(Enum_StatusCode statusCode, string? message = null)
            => new OperationResult(true, statusCode, message);
        public static OperationResult<TResult> Success<TResult>
            (TResult result, Enum_StatusCode statusCode, string? message = null)
            => new OperationResult<TResult>(result, true, statusCode, message);

        public static OperationResult Failure(Enum_StatusCode statusCode, string error) 
            => new OperationResult(false, statusCode, error);
        public static OperationResult<TResult> Failure<TResult>(Enum_StatusCode statusCode, string error) 
            => new OperationResult<TResult> (default, false, statusCode, error);

        public static OperationResult FromException(Exception ex) 
            => Failure(Enum_StatusCode.INTERNAL_SERVER_ERROR, ex.MessageWithInner());
        public static OperationResult<TResult> FromException<TResult>(Exception ex)
            => Failure<TResult>(Enum_StatusCode.INTERNAL_SERVER_ERROR, ex.MessageWithInner());
    }
}
