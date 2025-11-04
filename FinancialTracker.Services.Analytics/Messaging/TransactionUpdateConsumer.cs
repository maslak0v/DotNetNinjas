using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public sealed class TransactionUpdateConsumer(
    ILogger<TransactionUpdateConsumer> logger,
    IExpensesRepository expensesRepository,
    IIncomesRepository incomesRepository) : IConsumer<IUpdateTransactionMessage>
{
    public async Task Consume(ConsumeContext<IUpdateTransactionMessage> context)
    {
        var message = context.Message;
        logger.LogInformation(
            $"[RabbitMq] Transaction {message.TransactionId};\n" +
            $"User: {message.UserId}; Operation: {message.OperationType}");



        switch (message.OperationType?.ToLowerInvariant())
        {
            case "update":
                if (message.CategoryName.ToLowerInvariant() == "доход")
                    await incomesRepository.UpdateAsync(
                        message.TransactionId,
                        message.CategoryName,
                        "RUB",
                        message.Amount,
                        message.TransactionDate,
                        context.CancellationToken);
                else
                    await expensesRepository.UpdateAsync(
                        message.TransactionId,
                        message.CategoryName,
                        "RUB",
                        message.Amount,
                        message.TransactionDate,
                        context.CancellationToken); 
                    break;
            case "delete":
                if (message.CategoryName.ToLowerInvariant() == "доход")
                    await incomesRepository.DeleteByIdAsync(message.TransactionId, context.CancellationToken);
                else
                    await expensesRepository.DeleteByIdAsync(message.TransactionId, context.CancellationToken);
                break;
            default:
                logger.LogWarning($"Unknow operation: {message.OperationType}");
                break;
        }
    }
}