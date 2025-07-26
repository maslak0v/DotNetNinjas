namespace Wallet.Domain.Entities;

/// <summary>
/// Пользовательский тег.
/// </summary>
public class Tag
{
    public Guid TagId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}