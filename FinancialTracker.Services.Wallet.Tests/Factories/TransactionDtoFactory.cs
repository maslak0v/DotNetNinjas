using Wallet.Application.Dto.Transactions;
using Wallet.Domain.Enums;

namespace FinancialTracker.Services.Wallet.Tests.Factories;

public static class TransactionDtoFactory
{
    public static TransactionDto Create(
        Guid? accountId = null,
        decimal amount = 100m,
        OperationType operationType = OperationType.Income,
        string tag = "TestTag",
        int categoryId = 13)
    {
        return new TransactionDto
        {
            AccountId = accountId ?? Guid.NewGuid(),
            Amount = amount,
            OperationType = operationType,
            Tag = tag,
            CategoryId = categoryId
        };
    }
}