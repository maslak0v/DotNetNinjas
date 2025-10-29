using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class IncomeUpdateConsumer(
    ILogger<ExpenseUpdateConsumer> logger,
    IIncomesRepository incomesRepository) : IConsumer<IIncomeUpdateMessage>
{
    public async Task Consume(ConsumeContext<IIncomeUpdateMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation(
            $"[RabbitMQ] Income updated: {msg.IncomeId};" +
            $"User: {msg.UserId}");

        await incomesRepository.UpdateAsync(
            msg.IncomeId,
            msg.Category,
            msg.Currency,
            msg.Amount,
            msg.IncomeTime,
            context.CancellationToken);
    }
}
