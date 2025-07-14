using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Services;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class TransactionCreatedConsumer(
    ILogger<TransactionCreatedConsumer> logger,
    IExpensesService expensesService,
    IIncomesService incomesService) : IConsumer<ITransactionCreatedMessage>
{
    public async Task Consume(ConsumeContext<ITransactionCreatedMessage> context)
    {
        var message = context.Message;
        
        logger.LogInformation($"[RabbitMQ] Transaction created: UserId={message.UserId};" +
                              $"TransactionId={message.TransactionId}.\n" +
                              $"Info: {message.GetInfo()}"
                              );

        switch (message.OperationType)
        {
            case "Income":
                break;
            case "Expense":
                var expense = message.ToExpenseDto();
                await expensesService.AddAsync(expense);
                break;
            default:
                logger.LogError($"[TransactionCreatedConsumer] Invalid operation type: {message.OperationType}");
                break;
        };
    }

    private static string GetInfo(ITransactionCreatedMessage message)
    {
        var info = "";
        var messageType = message.GetType();
        foreach (var prop in messageType.GetProperties())
        {
            info += $"{prop.Name}={prop.GetValue(message)};";
        }

        return info;
    }
}