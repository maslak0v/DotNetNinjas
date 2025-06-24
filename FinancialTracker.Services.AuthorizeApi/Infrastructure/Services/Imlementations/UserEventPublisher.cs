using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces;
using MassTransit;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementations
{
    public class UserEventPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger<UserEventPublisher> logger)
        : IUserEventPublisher
    {
        public async Task Publish(IUserEvent userEvent)
        {
            logger.LogInformation(
                $"Publish event [{userEvent.GetType()}]: user[{userEvent.UserId}] created");
            await publishEndpoint.Publish(userEvent);
        }
    }
}
