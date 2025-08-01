using Wallet.Domain.Enums;

namespace Wallet.API.Models.Transactions;

public class TransactionResponse
{
    public Guid TransactionId { get; set; }
    public OperationType OperationType { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public string? Image { get; set; }
    public Guid? TagId { get; set; }
    public DateTime TransactionDate { get; set; }
}