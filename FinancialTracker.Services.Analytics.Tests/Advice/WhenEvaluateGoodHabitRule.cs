using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Advice.Rules;
using FinancialTracker.Services.Analytics.Tests.DSL;

namespace FinancialTracker.Services.Analytics.Tests.Advice;

public class WhenEvaluateGoodHabitRule
{
    [Test]
    public async Task ForAnyExpenses_ReturnsThatUserIsGoodGuy()
    {
        // Arrange
        var mary = CreateUser();
        var maryExpenses100 = Create.Expense()
            .Amount(100).For(mary).Please();
        var allExpenses = new List<Expense>
        {
            maryExpenses100
        };
        var rule = new GoodHabitRule();

        // Act
        var result = await rule.EvaluateAsync(allExpenses);

        // Assert
        Assert.That(result.Title, Is.EqualTo("Учёт трат"));
        Assert.That(result.Message, Is.EqualTo("Отлично, что фиксируешь траты!"));
    }
    
    [Test]
    public async Task ForNoExpenses_ReturnsEmptyResult()
    {
        // Arrange
        var noExpenses = new List<Expense>
        {
            /* empty */
        };
        var rule = new GoodHabitRule();

        // Act
        var result = await rule.EvaluateAsync(noExpenses);

        // Assert
        Assert.That(result.IsEmpty());
    }

    private User CreateUser()
    {
        var user = Create.User().Please();
        return user;
    }
}