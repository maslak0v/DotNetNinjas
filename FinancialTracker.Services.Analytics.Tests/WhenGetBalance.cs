using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetBalance
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ForAccountWithAnyExpenses_ResultShouldBeEqualToSumOfExpensesWithAMinusSign()
    {
        // Arrange
        var mockRepository = new Mock<IExpensesRepository>();
        var tommy = CreateUser("Tommy");
        var tommyExpenses10 = Create.Expense()
            .Amount(10).For(tommy).Please();
        var tommyExpenses100 = Create.Expense()
            .Amount(100).For(tommy).Please();
        var tommyExpenses1000 = Create.Expense()
            .Amount(1000).For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses10, tommyExpenses100, tommyExpenses1000
        };

        mockRepository.Setup(repo =>
                repo.GetExpensesBeforeDate(tommy.Guid, It.IsAny<DateTime>()))
            .Returns(allExpenses.AsQueryable);
        var expensesService = new ExpensesService(mockRepository.Object);
        var balanceService = new BalanceService(expensesService);
        
        // Act
        var result = balanceService.GetBalance(tommy.Guid,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(-(10+100+1000)));
    }

    [Test]
    public void ForAccountWithoutExpenses_ResultShouldBeZero()
    {
        // Arrange
        var mockRepository = new Mock<IExpensesRepository>();
        var tommy = CreateUser("Tommy");
        var allExpenses = new List<Expense> {};

        mockRepository.Setup(repo =>
                repo.GetExpensesBeforeDate(tommy.Guid, It.IsAny<DateTime>()))
            .Returns(allExpenses.AsQueryable);
        var expensesService = new ExpensesService(mockRepository.Object);
        var balanceService = new BalanceService(expensesService);
        
        // Act
        var result = balanceService.GetBalance(tommy.Guid,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    private User CreateUser(string name)
    {
        var user = Create.User().WithName(name).Please();
        return user;
    }
}