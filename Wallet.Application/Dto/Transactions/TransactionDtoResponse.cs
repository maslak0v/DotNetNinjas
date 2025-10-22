using Wallet.Domain.Enums;

namespace Wallet.Application.Dto.Transactions;

public class TransactionDtoResponse
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public OperationType OperationType { get; set; }
    public OperationType? OldOperationType { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public string? Image { get; set; }
    public string? TagName { get; set; }
    public Guid? TagId { get; set; }
}