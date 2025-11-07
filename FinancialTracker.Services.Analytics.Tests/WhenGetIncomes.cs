using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Services.Implementation;
using FinancialTracker.Services.Analytics.Tests.DSL;
using Moq;

namespace FinancialTracker.Services.Analytics.Tests;

public class WhenGetIncomes
{
    private readonly IMapper _mapper;

    public WhenGetIncomes()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<IncomeMappingsProfile>();
        });

        _mapper = config.CreateMapper();
    }
    
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task ForTommy_ReturnsIncomesOnlyForTommy()
    {
        // Arrange
        var mockRepository = new Mock<IIncomesRepository>();
        var tommy = CreateUser("Tommy");
        var alice = CreateUser("Alice");
        var tommyIncomes = Create.Income().Amount(1000).At(10, 3, 2020)
            .For(tommy).Please();
        var aliceIncomes = Create.Income().Amount(2000).At(11, 4, 2022)
            .For(alice).Please();
        var allIncomes = new List<Income>
        {
            tommyIncomes, aliceIncomes
        };

        mockRepository.Setup(repo =>
                repo.GetIncomesAsync(tommy.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(allIncomes.Where(e => e.User.Id == tommy.Id).ToList);
        var incomesService = new IncomesService(mockRepository.Object, _mapper);
        
        // Act
        var result = await incomesService.GetIncomesAsync(tommy.Id,
            new DateTime(2019, 01, 01),
            new DateTime(2024, 01, 01),
            CancellationToken.None);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First(), Is.EqualTo(tommyIncomes));
    }
    
    [Test]
    public async Task ForEmptyPeriod_ReturnEmptyIncomes()
    {
        // Arrange
        var tommy = CreateUser("Tommy");
        var emptyIncomes = new List<Income>();
        
        var mockRepository = new Mock<IIncomesRepository>();
        mockRepository.Setup(repo =>
                repo.GetIncomesAsync(tommy.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(emptyIncomes);
        var incomeService = new IncomesService(mockRepository.Object, _mapper);
        
        // Act
        var fromDate = new DateTime(2024, 01, 01);
        var toDate = fromDate.AddDays(-1);
        var result = await incomeService.GetIncomesAsync(tommy.Id,
            fromDate, toDate, CancellationToken.None);
        
        // Assert
        Assert.That(result, Is.EqualTo(emptyIncomes));
    }
    
    private User CreateUser(string name)
    {
        var user = Create.User().WithName(name).Please();
        return user;
    }
}