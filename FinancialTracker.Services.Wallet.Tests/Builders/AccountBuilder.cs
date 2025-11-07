using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
namespace FinancialTracker.Services.Wallet.Tests.Builders;

public class AccountBuilder
{
    private Guid _accountId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();
    private decimal _balance = 1000;
    private string _name = "Test Account";
    private Currency _currency = Currency.RUB;
    private List<Transaction> _transactions = new();
    private Guid _transactionId = Guid.NewGuid();

    public AccountBuilder WithId(Guid id)
    {
        _accountId = id;
        return this;
    }
    
    public AccountBuilder WithTransactionId(Guid transactionId)
    {
        _transactionId = transactionId;
        return this;
    }
    
    public AccountBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public AccountBuilder WithBalance(decimal balance)
    {
        _balance = balance;
        return this;
    }

    public AccountBuilder WithTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
        return this;
    }

    public Account Build()
    {
        return new Account
        {
            AccountId = _accountId,
            UserId = _userId,
            CurrentBalance = _balance,
            Name = _name,
            Currency = _currency,
            Transactions = _transactions
        };
    }
}