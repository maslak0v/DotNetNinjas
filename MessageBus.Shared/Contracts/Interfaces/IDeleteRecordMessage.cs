namespace MessageBus.Shared.Contracts.Interfaces
{
    public interface IDeleteTransactionMessage : IMessage
    {
        Guid TransactionId { get; }
    }
}
