using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Services;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class TransactionCreatedConsumer(
    ILogger<TransactionCreatedConsumer> logger,
    IExpensesService expensesService,
    IIncomesService incomesService) : IConsumer<ITransactionCreatedEvent>
{
    public async Task Consume(ConsumeContext<ITransactionCreatedEvent> context)
    {
        var message = context.Message;
        var messageType = message.GetType();
        var info = "";
        foreach (var prop in messageType.GetProperties())
        {
            info += $"{prop.Name}={prop.GetValue(message)};";
        }
        
        logger.LogInformation($"[RabbitMQ] Transaction created: UserId={message.UserId};" +
                              $"TransactionId={message.TransactionId}.\n" +
                              $"Info: {info}");
    }
}