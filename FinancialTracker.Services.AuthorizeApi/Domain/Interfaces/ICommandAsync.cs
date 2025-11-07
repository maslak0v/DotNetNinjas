namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces
{
    public interface ICommandAsync<TResult>
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
        TResult Result { get; }
    }
}
