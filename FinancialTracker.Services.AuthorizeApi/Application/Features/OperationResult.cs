using FinancialTracker.Services.AuthorizeApi.Application.Extensions;

namespace FinancialTracker.Services.AuthorizeApi.Application.Features
{
    public record OperationResult(bool IsSuccess, string? Message = null, string? Error = null);
    public record OperationResult<TResult>
        (TResult? Result, bool IsSuccess, string? Message = null, string? Error = null);
    public static class OperationResultCreator
    {
        public static OperationResult Success(string? message = null)
            => new OperationResult(true, message);
        public static OperationResult<TResult> Success<TResult>
            (TResult result, string? message = null)
            => new OperationResult<TResult>(result, true, message);

        public static OperationResult Failure(string error) 
            => new OperationResult(false, Error: error);
        public static OperationResult<TResult> Failure<TResult>(string error) 
            => new OperationResult<TResult> (IsSuccess: false, Error: error, Result: default);

        public static OperationResult FromException(Exception ex) 
            => Failure(ex.MessageWithInner());
        public static OperationResult<TResult> FromException<TResult>(Exception ex)
            => Failure<TResult>(ex.MessageWithInner());
    }
}
