namespace Wallet.Domain.Entities;

/// <summary>
/// Пользовательский тег.
/// </summary>
public class Tag
{
    public Guid TagId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}