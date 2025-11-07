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

public class TransactionServiceUpdateTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TransactionService _transactionService;
    private readonly AccountBuilder _accountBuilder;
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionServiceUpdateTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _transactionService = new TransactionService(_unitOfWorkMock.Object, _mapperMock.Object);
        _accountBuilder = new AccountBuilder();
        _transactionBuilder = new TransactionBuilder();
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesBalanceCorrectly()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var initialBalance = 2000m;
        var oldAmount = 500m;
        var newAmount = 700m;

        var transactionDto = TransactionDtoFactory.Create(
            accountId: accountId,
            amount: newAmount,
            operationType: OperationType.Expense);

        var account = _accountBuilder
            .WithId(accountId)
            .WithBalance(initialBalance)
            .Build();

        var existingTransaction = _transactionBuilder
            .WithId(transactionId)
            .WithAccount(account)
            .WithAmount(oldAmount)
            .WithOperationType(OperationType.Expense)
            .Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(existingTransaction);

        _unitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category());

        _mapperMock.Setup(x => x.Map<TransactionDtoResponse>(It.IsAny<Transaction>()))
            .Returns(new TransactionDtoResponse());

        // Act
        await _transactionService.UpdateAsync(transactionId, transactionDto, CancellationToken.None);
        
        // Assert
        // Initial: 2000
        // Remove old expense: 2000 + 500 = 2500
        // Add new expense: 2500 - 700 = 1800
        Assert.Equal(1800m, account.CurrentBalance);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingTag_ReusesTag()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
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

        var existingTransaction = _transactionBuilder
            .WithId(transactionId)
            .WithAccount(account)
            .Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(existingTransaction);

        _unitOfWorkMock.Setup(x => x.TagRepository.GetUserTagByNameAsync(
                existingTag.Name, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTag);

        // Act
        await _transactionService.UpdateAsync(transactionId, transactionDto, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.TagRepository.CreateAsync(
            It.IsAny<Tag>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateAsync_WithInvalidTransactionId_ReturnsNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transactionDto = TransactionDtoFactory.Create();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync((Transaction)null!);

        // Act
        var result = await _transactionService.UpdateAsync(transactionId, transactionDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.NotFound, result.StatusCode);
        Assert.Equal($"Транзакция не найдена: {transactionId}", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transactionDto = TransactionDtoFactory.Create();

        var existingTransaction = _transactionBuilder.Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(existingTransaction);

        _unitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null!);

        // Act
        var result = await _transactionService.UpdateAsync(transactionId, transactionDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.BadRequest, result.StatusCode);
        Assert.Equal($"Категория не найдена: {transactionDto.CategoryId}", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidOperationType_ReturnsBadRequest()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var invalidOperationType = (OperationType)24;

        var transactionDto = TransactionDtoFactory.Create(
            operationType: invalidOperationType);

        var existingTransaction = _transactionBuilder.Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(existingTransaction);

        // Act
        var result = await _transactionService.UpdateAsync(transactionId, transactionDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.BadRequest, result.StatusCode);
        Assert.Equal($"Недопустимый тип операции: {invalidOperationType}", result.Message);
    }
}