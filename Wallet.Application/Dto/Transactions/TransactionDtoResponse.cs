namespace Wallet.Application.Dto.Transactions;

public class TransactionDtoResponse
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public byte OperationType { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string? Comment { get; set; }
    public string? Image { get; set; }
    public string? Tag { get; set; }
}