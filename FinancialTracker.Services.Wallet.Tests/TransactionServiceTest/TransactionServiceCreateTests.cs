using AutoMapper;
using FinancialTracker.Services.Wallet.Tests.Builders;
using FinancialTracker.Services.Wallet.Tests.Factories;
using Moq;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Infrastructure.Services;

namespace FinancialTracker.Services.Wallet.Tests.TransactionServiceTest;

public class TransactionServiceCreateTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TransactionService _transactionService;
    private readonly AccountBuilder _accountBuilder;

    public TransactionServiceCreateTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _transactionService = new TransactionService(_unitOfWorkMock.Object, _mapperMock.Object);
        _accountBuilder = new AccountBuilder();
    }

    [Fact]
    public async Task CreateAsync_WithExpense_UpdatesBalanceCorrectly()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var initialBalance = 2000m;
        var expenseAmount = 500m;

        var transactionDto = TransactionDtoFactory.Create(
            accountId: accountId,
            amount: expenseAmount,
            operationType: OperationType.Expense);

        var account = _accountBuilder
            .WithId(accountId)
            .WithBalance(initialBalance)
            .Build();

        // Настройка моков
        _unitOfWorkMock.Setup(x => x.AccountRepository.GetByIdAsync(
                accountId, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(account);

        _unitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category());

        _unitOfWorkMock.Setup(x => x.TagRepository.GetUserTagByNameAsync(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag)null!);

        _mapperMock.Setup(x => x.Map<Transaction>(It.IsAny<TransactionDto>()))
            .Returns(new Transaction());

        // Act
        await _transactionService.CreateAsync(transactionDto, CancellationToken.None);
        //Assert
        Assert.Equal(initialBalance - expenseAmount, account.CurrentBalance); // 2000 - 500 = 1500
    }

    [Fact]
    public async Task CreateAsync_WithExistingTag_ReusesTag()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var existingTag = new Tag { TagId = Guid.NewGuid(), Name = "Food", UserId = userId };

        var transactionDto = TransactionDtoFactory.Create(
            accountId: accountId,
            tag: existingTag.Name);

        var account = _accountBuilder
            .WithId(accountId)
            .WithUserId(userId)
            .Build();

        _unitOfWorkMock.Setup(x => x.AccountRepository.GetByIdAsync(
                accountId, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(account);

        _unitOfWorkMock.Setup(x => x.TagRepository.GetUserTagByNameAsync(
                existingTag.Name, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTag);

        // Act
        var result = await _transactionService.CreateAsync(transactionDto, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.TagRepository.CreateAsync(
            It.IsAny<Tag>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidAccount_ReturnsNotFound()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var transactionDto = TransactionDtoFactory.Create(accountId);

        _unitOfWorkMock.Setup(x => x.AccountRepository.GetByIdAsync(
                accountId, It.IsAny<CancellationToken>(), false))
            .ReturnsAsync((Account)null!);

        // Act
        var result = await _transactionService.CreateAsync(transactionDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.NotFound, result.StatusCode);
        Assert.Equal($"Аккаунт не найден: {accountId}", result.Message);
    }
    
    [Theory]
    [InlineData(OperationType.Income, 1000, 500, true, 1500)]   // Income + adding = +amount
    [InlineData(OperationType.Income, 1000, 500, false, 500)]    // Income + removing = -amount
    [InlineData(OperationType.Expense, 1000, 500, true, 500)]     // Expense + adding = -amount
    [InlineData(OperationType.Expense, 1000, 500, false, 1500)]   // Expense + removing = +amount
    public void CalculateUpdatedBalance_CalculationsCorrect(
        OperationType operationType,
        decimal currentBalance,
        decimal amount,
        bool isAdding,
        decimal expected)
    {
        // Act
        var result = _transactionService.CalculateUpdatedBalance(
            currentBalance, amount, operationType, isAdding);

        // Assert
        Assert.Equal(expected, result);
    }
}