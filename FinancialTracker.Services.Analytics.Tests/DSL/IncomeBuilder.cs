using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.Tests.DSL;

public class IncomeBuilder
{
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly int _incomeId = 0;
    private decimal _amount = 0;
    private DateTime _dateTime;
    private User _user = new User();

    public IncomeBuilder Amount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public IncomeBuilder At(int day, int month, int year)
    {
        _dateTime = new DateTime(year, month, day);
        return this;
    }

    public IncomeBuilder For(User user)
    {
        _user = user;
        return this;
    }

    public Income Please()
    {
        return new Income()
        {
            AccountId = _accountId,
            IncomeId = _incomeId,
            Amount = _amount,
            IncomeTime = _dateTime,
            User = _user
        };
    }
}