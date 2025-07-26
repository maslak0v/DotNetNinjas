using MessageBus.Shared.Contracts.Interfaces;

namespace MessageBus.Shared.Contracts.Implementations;

public class TransactionEvents
{
    public record TransactionDeletedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid TransactionId,
        String TransactionType
    ) : IDeleteTransactionMessage;
    
    public record IncomeCreatedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid IncomeId,
        Guid UserId,
        Guid AccountId,
        string CategoryName,
        DateTime IncomeTime,
        decimal Amount) : IIncomeCreatedMessage;


    public record ExpenseCreatedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid ExpenseId,
        Guid UserId,
        Guid AccountId,
        string CategoryName,
        DateTime ExpenseTime,
        decimal Amount) : IExpenseCreatedMessage;
}
