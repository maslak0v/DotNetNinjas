namespace Wallet.API.Models.Accounts;

public class UpdateAccountRequest
{
        public string Name { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
}