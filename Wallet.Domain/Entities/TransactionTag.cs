namespace Wallet.Domain.Entities;

public class TransactionTag
{
    public Guid TransactionTagId { get; set; } = Guid.NewGuid();

    public Guid TransactionId { get; set; }
    public Guid TagId { get; set; }

    public Transaction Transaction { get; set; }
    public Tag Tag { get; set; }
}