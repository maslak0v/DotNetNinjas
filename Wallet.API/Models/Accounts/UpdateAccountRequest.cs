using System.ComponentModel.DataAnnotations;

namespace Wallet.API.Models.Accounts;

public class UpdateAccountRequest
{
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
}