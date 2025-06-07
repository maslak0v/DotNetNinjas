using Wallet.Domain.Enums;

namespace Wallet.API.Models.Accounts;

public class AccountRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public Currency Currency { get; set; }
}