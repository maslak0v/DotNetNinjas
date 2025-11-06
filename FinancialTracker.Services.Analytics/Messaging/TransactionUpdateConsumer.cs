using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public sealed class TransactionUpdateConsumer(
 ILogger<TransactionUpdateConsumer> logger,
 IExpensesRepository expensesRepository,
 IIncomesRepository incomesRepository) : IConsumer<IUpdateTransactionMessage>
{
    private const string DefaultCurrency = "RUB";

    // словарь действий
    private readonly IReadOnlyDictionary<OperationType, Func<IUpdateTransactionMessage, CancellationToken, Task>> _actions =
        new Dictionary<OperationType, Func<IUpdateTransactionMessage, CancellationToken, Task>>
        {
            [OperationType.Income] = (m, ct) => incomesRepository.UpdateAsync(
                m.TransactionId, m.CategoryName, DefaultCurrency, m.Amount, m.TransactionDate, ct),

            [OperationType.Expense] = (m, ct) => expensesRepository.UpdateAsync(
                m.TransactionId, m.CategoryName, DefaultCurrency, m.Amount, m.TransactionDate, ct),

            [OperationType.IncomeToExpense] = async (m, ct) =>
            {
                await incomesRepository.DeleteByIdAsync(m.TransactionId, ct);
                await expensesRepository.AddAsync(CreateTransactionEntity<Expense>(m)!, ct);
            },

            [OperationType.ExpenseToIncome] = async (m, ct) =>
            {
                await expensesRepository.DeleteByIdAsync(m.TransactionId, ct);
                await incomesRepository.AddAsync(CreateTransactionEntity<Income>(m)!, ct);
            }
        };

    public async Task Consume(ConsumeContext<IUpdateTransactionMessage> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "[RabbitMq] Transaction {TransactionId} User: {UserId}; Operation: {OperationType} OldOperation: {OldOperationType}",
            message.TransactionId, message.UserId, message.OperationType, message.OldOperationType);

        var operationType = CalcOperationType(message.OperationType, message.OldOperationType);

        if (_actions.TryGetValue(operationType, out var action))
        {
            await action(message, context.CancellationToken);
        }
        else
        {
            logger.LogWarning("Unknown operation type: {OperationType}", operationType);
        }
    }

    private static OperationType CalcOperationType(string current, string old) =>
        (current.Equals("income", StringComparison.OrdinalIgnoreCase),
         old.Equals("income", StringComparison.OrdinalIgnoreCase)) switch
        {
            (true, true) => OperationType.Income,
            (false, false) => OperationType.Expense,
            (false, true) => OperationType.IncomeToExpense,
            (true, false) => OperationType.ExpenseToIncome
        };

    private static T? CreateTransactionEntity<T>(IUpdateTransactionMessage m) where T : class, new() =>
        typeof(T) == typeof(Expense)
            ? new Expense
            {
                ExpenseId = m.TransactionId,
                AccountId = m.AccountId,
                Amount = m.Amount,
                Category = m.CategoryName,
                Currency = DefaultCurrency,
                ExpenseTime = m.TransactionDate,
                UserId = m.UserId
            } as T
            : new Income
            {
                IncomeId = m.TransactionId,
                AccountId = m.AccountId,
                Amount = m.Amount,
                Category = m.CategoryName,
                Currency = DefaultCurrency,
                IncomeTime = m.TransactionDate,
                UserId = m.UserId
            } as T;

    private enum OperationType
    {
        Income,
        Expense,
        IncomeToExpense,
        ExpenseToIncome
    }
}