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
    public void ReturnsExpensesOnlyForTommy()
    {
        // Arrange
        var mockRepository = new Mock<IExpensesRepository>();
        var tommy = Create.User().WithName("Tommy").Please();
        var alice = Create.User().WithName("Alice").Please();
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
}