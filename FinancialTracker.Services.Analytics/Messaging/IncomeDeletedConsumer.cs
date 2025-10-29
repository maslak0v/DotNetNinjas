using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public sealed class IncomeDeletedConsumer(
    ILogger<IncomeDeletedConsumer> logger,
    IIncomesRepository incomesRepository) : IConsumer<IIncomeDeletedMessage>
{
    public async Task Consume(ConsumeContext<IIncomeDeletedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation(
            $"[RabbitMQ] Income deleted: {message.IncomeId};" +
            $"User: {message.UserId}");

        await incomesRepository.DeleteByIdAsync(message.IncomeId, context.CancellationToken);
    }
}
