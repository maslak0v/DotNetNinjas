using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class ExpenseUpdateConsumer(
    ILogger<ExpenseUpdateConsumer> logger,
    IExpensesRepository expensesRepository) : IConsumer<IExpenseUpdateMessage>
{
    public async Task Consume(ConsumeContext<IExpenseUpdateMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation(
            $"[RabbitMQ] Expense updated: {msg.ExpenseId};" +
            $"User: {msg.UserId}");

        await expensesRepository.UpdateAsync(
            msg.ExpenseId,
            msg.Category,
            msg.Currency,
            msg.Amount,
            msg.ExpenseTime,
            context.CancellationToken);
    }
}
