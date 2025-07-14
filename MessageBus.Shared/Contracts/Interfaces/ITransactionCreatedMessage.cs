namespace MessageBus.Shared.Contracts.Interfaces;

public interface ITransactionCreatedMessage : IMessage
{
    public Guid UserId { get; }
    
    public Guid AccountId { get; }
    
    public Guid TransactionId { get; }

    public string OperationType { get; }

    public int CategoryId { get; }

    public string CategoryName { get; }

    public DateTime TransactionDate { get; }
    
    public decimal Amount { get; }
}