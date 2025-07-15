using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetBalance
{
    private Mock<IExpensesService> _expensesServiceMock;
    private Mock<IIncomesService> _incomesServiceMock;
    private BalanceService _balanceService;

    [SetUp]
    public void Setup()
    {
        _expensesServiceMock = new Mock<IExpensesService>();
        _incomesServiceMock = new Mock<IIncomesService>();
        _balanceService = new BalanceService(_expensesServiceMock.Object, _incomesServiceMock.Object);
    }

    [Test]
    public async Task ForAccountWithAnyExpenses_ResultShouldBeEqualToSumOfExpensesWithAMinusSign()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        var tommyExpenses10 = Create.Expense().Amount(10).For(tommy).Please();
        var tommyExpenses100 = Create.Expense().Amount(100).For(tommy).Please();
        var tommyExpenses1000 = Create.Expense().Amount(1000).For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses10, tommyExpenses100, tommyExpenses1000
        };

        _expensesServiceMock.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allExpenses);
        _incomesServiceMock.Setup(repo =>
                repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Income>());
        
        // Act
        var result = await _balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(-(10+100+1000)));
    }

    [Test]
    public async Task ForAccountWithAnyIncome_ResultShouldBeEqualToSumOfIncome()
    {
        //  Arrange
        var tommy = CreateUser("Tommy");
        var tommyIncome10 = Create.Income().Amount(10).For(tommy).Please();
        var tommyIncome100 = Create.Income().Amount(100).For(tommy).Please();
        var tommyIncome1000 = Create.Income().Amount(1000).For(tommy).Please();
        var allIncomes = new List<Income>
        {
            tommyIncome10, tommyIncome100, tommyIncome1000
        };

        _expensesServiceMock.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Expense>());
        _incomesServiceMock.Setup(repo =>
                repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allIncomes);

        // Act
        var result = await _balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(10 + 100 + 1000));
    }

    [Test]
    public async Task ForAccountWithBothExpensesAndIncomes_ResultShouldBeDifferenceBetweenIncomesAndExpenses()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        
        var tommyExpenses10 = Create.Expense().Amount(10).For(tommy).Please();
        var tommyExpenses100 = Create.Expense().Amount(100).For(tommy).Please();
        var tommyExpenses1000 = Create.Expense().Amount(1000).For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses10, tommyExpenses100, tommyExpenses1000
        };

        var tomyIncome5000 = Create.Income().Amount(5000).For(tommy).Please();
        var allIncome = new List<Income> { tomyIncome5000 };

        _expensesServiceMock.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allExpenses);
        _incomesServiceMock.Setup(repo =>
                repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allIncome);

        // act
        var result = await _balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(5000 - (10 + 100 + 1000)));
    }

    [Test]
    public async Task ForAccountWithoutExpenses_ResultShouldBeZero()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        var allExpenses = new List<Expense> { };
        var allIncomes = new List<Income> { };

        _expensesServiceMock.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allExpenses);
        _incomesServiceMock.Setup(repo =>
                repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allIncomes);

        // Act
        var result = await _balanceService.GetBalanceAsync(tommy.Id,
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