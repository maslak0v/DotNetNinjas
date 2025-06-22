namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces
{
    public interface IEventUserCreated
    {
        Guid Id { get; }
        DateTime Created { get; }
        string UserName { get; }
    }
}
