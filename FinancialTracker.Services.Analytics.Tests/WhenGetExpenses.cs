using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetExpenses
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ForTommy_ReturnsExpensesOnlyForTommy()
    {
        // Arrange
        var mockRepository = new Mock<IExpensesRepository>();
        var tommy = CreateUser("Tommy");
        var alice = CreateUser("Alice");
        var tommyExpenses = Create.Expense().Amount(100).At(10, 3, 2020)
            .For(tommy).Please();
        var aliceExpenses = Create.Expense().Amount(200).At(11, 4, 2022)
            .For(alice).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses, aliceExpenses
        };

        mockRepository.Setup(repo =>
                repo.GetExpenses(tommy.Guid, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(allExpenses.Where(e => e.User.Guid == tommy.Guid));
        var expensesService = new ExpensesService(mockRepository.Object);
        
        // Act
        var result = expensesService.GetExpenses(tommy.Guid,
            new DateTime(2019, 01, 01),
            new DateTime(2024, 01, 01)).ToList();

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First(), Is.EqualTo(tommyExpenses));
    }

    [Test]
    public void ForEmptyPeriod_ReturnEmptyExpenses()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        var emptyExpenses = new List<Expense>();
        
        var mockRepository = new Mock<IExpensesRepository>();
        mockRepository.Setup(repo =>
                repo.GetExpenses(tommy.Guid, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(emptyExpenses);
        var expensesService = new ExpensesService(mockRepository.Object);
        
        // Act
        var fromDate = new DateTime(2024, 01, 01);
        var toDate = fromDate.AddDays(-1);
        var result = expensesService.GetExpenses(tommy.Guid,
            fromDate, toDate)
            .ToList();
        
        // Assert
        Assert.That(result, Is.EqualTo(emptyExpenses));
    }
    
    private User CreateUser(string name)
    {
        var user = Create.User().WithName(name).Please();
        return user;
    }
}