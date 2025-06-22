using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces;
using MassTransit;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementations
{
    public class UserEventPublishService(
        IPublishEndpoint publishEndpoint,
        ILogger<UserEventPublishService> logger)
        : IUserEventPublishService
    {
        public async Task PublishEventUserCreated(IEventUserCreated eventUserCreated)
        {
            logger.LogInformation($"Publish event: user[{eventUserCreated.Id}] created");
            await publishEndpoint.Publish(eventUserCreated);
        }
    }
}
