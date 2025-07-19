namespace MessageBus.Shared.Contracts.Interfaces;

public interface IIncomeCreatedMessage : IMessage
{
    public Guid IncomeId { get; }
    public Guid UserId { get; }
    public Guid AccountId { get; }
    public string CategoryName { get; }
    public DateTime IncomeTime { get; }
    public decimal Amount { get; }
}