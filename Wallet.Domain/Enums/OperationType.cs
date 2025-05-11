namespace Wallet.Domain.Enums;
/// <summary>
/// Тип операции: приход или расход.
/// </summary>
public enum OperationType : byte
{
    /// <summary>
    /// Приход (доход).
    /// </summary>
    Income = 1,

    /// <summary>
    /// Расход.
    /// </summary>
    Expense = 2
}