using Wallet.Domain.Enums;

namespace Wallet.API.Models.Accounts;

public class AccountResponse
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public Currency Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}