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
        var sumOfAllExpenses = 1_000;
        var sumOfAllIncomes = 0;

        mockExpenses.Setup(repo =>
                repo.GetSumOfExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(sumOfAllExpenses);
        mockIncomes.Setup(repo =>
               repo.GetSumOfIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(sumOfAllIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);
        
        // Act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01),
            CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(-1_000));
    }

    [Test]
    public async Task ForAccountWithAnyIncome_ResultShouldBeEqualToSumOfIncome()
    {
        //  Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        
        var sumOfAllExpenses = 0;
        var sumOfAllIncomes = 4_000;
        
        mockExpenses.Setup(repo =>
                repo.GetSumOfExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(sumOfAllExpenses);
        mockIncomes.Setup(repo =>
               repo.GetSumOfIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(sumOfAllIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);
        // Act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01), CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(4_000));
    }

    [Test]
    public async Task ForAccountWithBothExpensesAndIncomes_ResultShouldBeDifferenceBetweenIncomesAndExpenses()
    {
        // Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        
        var sumOfAllExpenses = 1_111;
        var sumOfAllIncomes = 5_000;

        mockExpenses.Setup(repo =>
                repo.GetSumOfExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(sumOfAllExpenses);
        mockIncomes.Setup(repo =>
               repo.GetSumOfIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(sumOfAllIncomes);
        var expensesService = new ExpensesService(mockExpenses.Object, _mapper);
        var incomesService = new IncomesService(mockIncomes.Object, _mapper);
        var balanceService = new BalanceService(expensesService, incomesService);

        // act
        var result = await balanceService.GetBalanceAsync(tommy.Id,
            new DateTime(2010, 01, 01), CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(5_000 - 1_111));
    }

    [Test]
    public async Task ForAccountWithoutExpenses_ResultShouldBeZero()
    {
        // Arrange
        var mockExpenses = new Mock<IExpensesRepository>();
        var mockIncomes = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        var sumOfAllExpenses = 0;
        var sumOfAllIncomes = 0;

        mockExpenses.Setup(repo =>
                repo.GetSumOfExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(sumOfAllExpenses);
        mockIncomes.Setup(repo =>
               repo.GetSumOfIncomesUpToDateAsync(tommy.Id, It.IsAny<DateTime>(), CancellationToken.None))
           .ReturnsAsync(sumOfAllIncomes);
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