using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces
{
    public interface IUserEventPublishService
    {
        Task PublishEventUserCreated(IEventUserCreated eentUserCreated);
    }
}
