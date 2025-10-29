using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class UserDeletedAnalyticsConsumer(
    ILogger<UserDeletedAnalyticsConsumer> logger,
    IUserRepository userRepository) : IConsumer<IUserDeleted>
{
    public async Task Consume(ConsumeContext<IUserDeleted> context)
    {
        var msg = context.Message;
        logger.LogInformation("[RabbitMQ] User deleted: {userId}", msg.UserId);
        await userRepository.DeleteByIdAsync(msg.UserId, context.CancellationToken);
    }
}
