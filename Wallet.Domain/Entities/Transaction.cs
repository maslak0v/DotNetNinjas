using Wallet.Domain.Enums;

namespace Wallet.Domain.Entities;
/// <summary>
/// Финансовая операция.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Уникальный идентификатор транзакции.
    /// </summary>
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Тип операции (например, доход или расход).
    /// </summary>
    public OperationType OperationType { get; set; }

    /// <summary>
    /// Идентификатор категории транзакции.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Идентификатор счета, к которому относится транзакция.
    /// </summary>
    public Guid WalletId { get; set; }

    /// <summary>
    /// Сумма транзакции.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Дата совершения транзакции.
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Комментарий к транзакции.
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Путь к изображению, связанному с транзакцией.
    /// </summary>
    public string Image { get; set; }
    
    /// <summary>
    /// Дата последнего изменения транзакции.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Признак удаления транзакции.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Счет, к которому относится транзакция.
    /// </summary>
    public Account Account { get; set; }

    /// <summary>
    /// Категория транзакции.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Связанные теги транзакции.
    /// </summary>
    public ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}