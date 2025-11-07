namespace MessageBus.Shared.Contracts.Interfaces
{
    public interface IUpdateTransactionMessage : IMessage
    {
        public Guid TransactionId { get; }
        public Guid UserId { get; }
        public Guid AccountId { get; }
        public string CategoryName { get; }
        public DateTime TransactionDate { get; }
        public decimal Amount { get; }
        public string OperationType { get;}
        public string OldOperationType { get;}
    }
}
