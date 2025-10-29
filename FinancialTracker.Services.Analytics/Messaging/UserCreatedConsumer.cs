using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class UserCreatedConsumer(
    ILogger<UserCreatedConsumer> logger,
    IUserRepository userRepository) : IConsumer<IUserCreated>
{
    public async Task Consume(ConsumeContext<IUserCreated> context)
    {
        logger.LogInformation($"[RabbitMQ] User created: {context.Message.UserId}");
        var user = new User
        {
            Id = context.Message.UserId,
            Name = context.Message.Name
        };

        await userRepository.AddAsync(user, context.CancellationToken);

    }
}
