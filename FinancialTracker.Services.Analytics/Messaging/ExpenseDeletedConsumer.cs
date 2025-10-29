using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public sealed class ExpenseDeletedConsumer(
    ILogger<ExpenseDeletedConsumer> logger,
    IExpensesRepository expensesRepository) : IConsumer<IExpenseDeletedMessage>
{
    public async Task Consume(ConsumeContext<IExpenseDeletedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation(
            $"[RabbitMQ] Expense deleted: {message.ExpenseId};" +
            $"User: {message.UserId}");

        await expensesRepository.DeleteByIdAsync(message.ExpenseId, context.CancellationToken);
    }
}
