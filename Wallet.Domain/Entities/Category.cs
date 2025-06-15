namespace Wallet.Domain.Entities;

/// <summary>
/// Категория для транзакций ("Еда", "Транспорт", "Зарплата" и тд).
/// </summary>
public class Category
{
    /// <summary>
    /// ИД.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Название иконки для категории.
    /// </summary>
    public string Icon { get; set; } = "default";

    /// <summary>
    /// Дата и время создания категории в UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время последнего обновления категории в UTC.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Список транзакций, связанных с этой категорией.
    /// </summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}