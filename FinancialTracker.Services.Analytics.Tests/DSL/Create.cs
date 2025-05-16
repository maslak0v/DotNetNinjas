namespace FinancialTracker.Services.Analytics.Tests.DSL;

public class Create 
{
    public static UserBuilder User()
    {
        return new UserBuilder();
    }

    public static ExpenseBuilder Expense()
    {
        return new ExpenseBuilder();
    }
}