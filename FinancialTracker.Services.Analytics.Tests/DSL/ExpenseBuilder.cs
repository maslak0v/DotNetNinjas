using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.Tests.DSL;

public class ExpenseBuilder
{
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly int _expenseId = 0;
    private decimal _amount = 0;
    private DateTime _dateTime;
    private User _user = new User();

    public ExpenseBuilder Amount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseBuilder At(int day, int month, int year)
    {
        _dateTime = new DateTime(year, month, day);
        return this;
    }

    public ExpenseBuilder For(User user)
    {
        _user = user;
        return this;
    }

    public Expense Please()
    {
        return new Expense()
        {
            AccountId = _accountId,
            ExpenseId = _expenseId,
            Amount = _amount,
            ExpenseTime = _dateTime,
            User = _user
        };
    }
}