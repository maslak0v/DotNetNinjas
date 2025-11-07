namespace MessageBus.Shared.Contracts.Interfaces;

public interface IExpenseCreatedMessage : IMessage
{
    public Guid ExpenseId { get; }
    public Guid UserId { get; }
    public Guid AccountId { get; }
    public string CategoryName { get; }
    public DateTime ExpenseTime { get; }
    public decimal Amount { get; }
}