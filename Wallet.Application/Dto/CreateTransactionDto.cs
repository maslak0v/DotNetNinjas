using Wallet.Domain.Enums;

namespace Wallet.Application.Dto;

public class TransactionDto
{
    public Guid AccountId { get; set; }
    public OperationType OperationType { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? Tag { get; set; }
}