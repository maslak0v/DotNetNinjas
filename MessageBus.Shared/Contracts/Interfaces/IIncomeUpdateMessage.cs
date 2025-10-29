namespace MessageBus.Shared.Contracts.Interfaces;

public interface IIncomeUpdateMessage
{
    Guid IncomeId { get; }
    Guid UserId { get; }
    Guid AccountId { get; }
    string? Category { get; }
    string Currency { get; }
    decimal Amount { get; }
    DateTime IncomeTime { get; }
    Guid MessageId { get; }
    DateTime UpdateTime { get; }
}
