using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetBalance
{
    private readonly IMapper _mapper;
    
    public WhenGetBalance()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ExpenseMappingsProfile>();
        });

        _mapper = config.CreateMapper();
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ForAccountWithAnyExpenses_ResultShouldBeEqualToSumOfExpensesWithAMinusSign()
    {
        // Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        var tommyExpenses10 = Create.Expense().Amount(10).For(tommy).Please();
        var tommyExpenses100 = Create.Expense().Amount(100).For(tommy).Please();
        var tommyExpenses1000 = Create.Expense().Amount(1000).For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses10, tommyExpenses100, tommyExpenses1000
        };
        var allIncomes = new List<Income>();

        mockExpenses.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(allExpenses);
        mockIncomes.Setup(repo =>
               repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(allIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);
        
        // Act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01),
            CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(-(10+100+1000)));
    }

    [Test]
    public async Task ForAccountWithAnyIncome_ResultShouldBeEqualToSumOfIncome()
    {
        //  Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        var tommyIncome10 = Create.Income().Amount(10).For(tommy).Please();
        var tommyIncome100 = Create.Income().Amount(100).For(tommy).Please();
        var tommyIncome1000 = Create.Income().Amount(1000).For(tommy).Please();
        var allIncomes = new List<Income>
        {
            tommyIncome10, tommyIncome100, tommyIncome1000
        };
        mockExpenses.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(new List<Expense>());
        mockIncomes.Setup(repo =>
               repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(allIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);
        // Act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01), CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(10 + 100 + 1000));
    }

    [Test]
    public async Task ForAccountWithBothExpensesAndIncomes_ResultShouldBeDifferenceBetweenIncomesAndExpenses()
    {
        // Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        
        var tommyExpenses10 = Create.Expense().Amount(10).For(tommy).Please();
        var tommyExpenses100 = Create.Expense().Amount(100).For(tommy).Please();
        var tommyExpenses1000 = Create.Expense().Amount(1000).For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses10, tommyExpenses100, tommyExpenses1000
        };

        var tomyIncome5000 = Create.Income().Amount(5000).For(tommy).Please();
        var allIncomes = new List<Income> { tomyIncome5000 };

        mockExpenses.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(allExpenses);
        mockIncomes.Setup(repo =>
               repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(allIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);

        // act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01), CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(5000 - (10 + 100 + 1000)));
    }

    [Test]
    public async Task ForAccountWithoutExpenses_ResultShouldBeZero()
    {
        // Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        var allExpenses = new List<Expense> { };
        var allIncomes = new List<Income> { };

        mockExpenses.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(allExpenses);
        mockIncomes.Setup(repo =>
               repo.GetIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(allIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);
        
        // Act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01),
            CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    private User CreateUser(string name)
    {
        var user = Create.User().WithName(name).Please();
        return user;
    }
}