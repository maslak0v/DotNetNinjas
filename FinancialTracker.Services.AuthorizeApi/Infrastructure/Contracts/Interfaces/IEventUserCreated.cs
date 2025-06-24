namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces
{
    public interface IEventUserCreated: IUserEvent
    {
        DateTime Created { get; }
        string UserName { get; }
    }
}
