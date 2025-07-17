using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetExpenses
{
    private readonly IMapper _mapper;
    
    public WhenGetExpenses()
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
    public async Task ForTommy_ReturnsExpensesOnlyForTommy()
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
                repo.GetExpensesAsync(tommy.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(allExpenses.Where(e => e.User.Id == tommy.Id).ToList);
        var expensesService = new ExpensesService(mockRepository.Object, _mapper);
        
        // Act
        var result = await expensesService.GetExpensesAsync(tommy.Id,
            new DateTime(2019, 01, 01),
            new DateTime(2024, 01, 01));

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First(), Is.EqualTo(tommyExpenses));
    }

    [Test]
    public async Task ForEmptyPeriod_ReturnEmptyExpenses()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        var emptyExpenses = new List<Expense>();
        
        var mockRepository = new Mock<IExpensesRepository>();
        mockRepository.Setup(repo =>
                repo.GetExpensesAsync(tommy.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(emptyExpenses);
        var expensesService = new ExpensesService(mockRepository.Object, _mapper);
        
        // Act
        var fromDate = new DateTime(2024, 01, 01);
        var toDate = fromDate.AddDays(-1);
        var result = await expensesService.GetExpensesAsync(tommy.Id,
            fromDate, toDate);
        
        // Assert
        Assert.That(result, Is.EqualTo(emptyExpenses));
    }

    [Test]
    public async Task UpToDate_ReturnsExpensesOnlyBeforeDateAndOnDate()
    {
        // Arrange
        var mockRepository = new Mock<IExpensesRepository>();
        var tommy = CreateUser("Tommy");
        var tommyExpenses2009 = Create.Expense()
            .At(01, 01, 2009)
            .For(tommy).Please();
        var tommyExpenses2010 = Create.Expense()
            .At(01, 01, 2010)
            .For(tommy).Please();
        var tommyExpenses2011 = Create.Expense()
            .At(01, 01, 2011)
            .For(tommy).Please();
        var allExpenses = new List<Expense>
        {
            tommyExpenses2009, tommyExpenses2010, tommyExpenses2011
        };
        var expensesUpTo2010 = new List<Expense>
        {
            tommyExpenses2009, tommyExpenses2010
        };

        mockRepository.Setup(repo =>
                repo.GetExpensesUpToDateAsync(tommy.Id, It.IsAny<DateTime>()))
            .ReturnsAsync(allExpenses.Where(e => e.ExpenseTime.Year <= 2010).ToList());
        var expensesService = new ExpensesService(mockRepository.Object, _mapper);
        
        // Act
        var result = await expensesService.GetExpensesUpToDateAsync(tommy.Id,
            new DateTime(2010, 01, 01));

        // Assert
        Assert.That(result, Is.EqualTo(expensesUpTo2010));
    }
    
    private User CreateUser(string name)
    {
        var user = Create.User().WithName(name).Please();
        return user;
    }
}