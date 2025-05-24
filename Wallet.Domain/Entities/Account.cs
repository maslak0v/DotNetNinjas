using Wallet.Domain.Enums;

namespace Wallet.Domain.Entities;

/// <summary>
/// Кошелек.
/// </summary>
public class Account
{
    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    public Guid AccountId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит счет.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Название счета.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Текущий баланс счета.
    /// </summary>
    public decimal CurrentBalance { get; set; } = 0;

    /// <summary>
    /// Валюта счета.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Дата создания счета.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего изменения счета.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Признак удаления счета.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Коллекция транзакций, связанных с этим счетом.
    /// </summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}