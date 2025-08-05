using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace FinancialTracker.Services.Wallet.Tests.Builders;

public class TransactionBuilder
{
    private Guid _transactionId = Guid.NewGuid();
    private Guid _accountId;
    private decimal _amount = 100m;
    private OperationType _operationType = OperationType.Income;
    private readonly int _categoryId = 13;
    private Guid? _tagId = Guid.NewGuid();
    private DateTime _transactionDate = DateTime.UtcNow;
    private Account _account;
    
    public TransactionBuilder WithId(Guid transactionId)
    {
        _transactionId = transactionId;
        return this;
    }

    public TransactionBuilder WithAccountId(Guid accountId)
    {
        _accountId = accountId;
        return this;
    }

    public TransactionBuilder WithAccount(Account account)
    {
        _account = account;
        _accountId = account.AccountId;
        return this;
    }

    public TransactionBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public TransactionBuilder WithOperationType(OperationType type)
    {
        _operationType = type;
        return this;
    }

    public TransactionBuilder WithTagId(Guid? tagId)
    {
        _tagId = tagId;
        return this;
    }

    public TransactionBuilder WithTransactionDate(DateTime date)
    {
        _transactionDate = date;
        return this;
    }

    public Transaction Build()
    {
        return new Transaction
        {
            TransactionId = _transactionId,
            AccountId = _accountId,
            Account = _account,
            Amount = _amount,
            OperationType = _operationType,
            CategoryId = _categoryId,
            TagId = _tagId,
            TransactionDate = _transactionDate,
            UpdatedAt = DateTime.UtcNow
        };
    }
}