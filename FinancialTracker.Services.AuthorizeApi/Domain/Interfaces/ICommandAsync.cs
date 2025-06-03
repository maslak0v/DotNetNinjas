namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces
{
    public interface ICommandAsync<TResult>
    {
        Task ExecuteAsync();
        TResult Result { get; }
    }
}
