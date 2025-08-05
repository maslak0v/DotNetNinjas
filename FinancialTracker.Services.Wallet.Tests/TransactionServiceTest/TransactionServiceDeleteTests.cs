using AutoMapper;
using FinancialTracker.Services.Wallet.Tests.Builders;
using Moq;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Infrastructure.Services;

namespace FinancialTracker.Services.Wallet.Tests.TransactionServiceTest;

public class TransactionServiceDeleteTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TransactionService _transactionService;
    private readonly AccountBuilder _accountBuilder;
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionServiceDeleteTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _transactionService = new TransactionService(_unitOfWorkMock.Object, _mapperMock.Object);
        _accountBuilder = new AccountBuilder();
        _transactionBuilder = new TransactionBuilder();
    }
    
    [Fact]
    public async Task DeleteAsync_WithValidTransaction_UpdatesBalanceAndMarksAsDeleted()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var initialBalance = 2000m;
        var transactionAmount = 500m;
        var operationType = OperationType.Expense;
        var expectedBalance = initialBalance + transactionAmount; // Возвращаем сумму обратно

        var account = _accountBuilder
            .WithId(accountId)
            .WithBalance(initialBalance)
            .Build();

        var transaction = _transactionBuilder
            .WithId(transactionId)
            .WithAmount(transactionAmount)
            .WithOperationType(operationType)
            .WithAccount(account)
            .WithAccountId(accountId)
            .Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(transaction);

        _mapperMock.Setup(x => x.Map<DeleteTransactionDto>(It.IsAny<Transaction>()))
            .Returns(new DeleteTransactionDto());

        // Act
        var result = await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedBalance, account.CurrentBalance);
        Assert.True(transaction.IsDeleted);
        Assert.NotNull(transaction.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_WithIncomeTransaction_UpdatesBalanceCorrectly()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var initialBalance = 2000m;
        var transactionAmount = 500m;
        var operationType = OperationType.Income;
        var expectedBalance = initialBalance - transactionAmount; // Отнимаем сумму дохода

        var account = _accountBuilder
            .WithId(accountId)
            .WithBalance(initialBalance)
            .Build();

        var transaction = _transactionBuilder
            .WithId(transactionId)
            .WithAmount(transactionAmount)
            .WithOperationType(operationType)
            .WithAccount(account)
            .WithAccountId(accountId)
            .Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(transaction);

        // Act
        await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.Equal(expectedBalance, account.CurrentBalance);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingTransaction_ReturnsNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync((Transaction)null!);

        // Act
        var result = await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.NotFound, result.StatusCode);
        Assert.Equal($"Транзакция не найдена: {transactionId}", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WithExpenseTransaction_UpdatesBalanceCorrectly()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var initialBalance = 2000m;
        var transactionAmount = 500m;
        var operationType = OperationType.Expense;
        var expectedBalance = initialBalance + transactionAmount;

        var account = _accountBuilder
            .WithId(accountId)
            .WithBalance(initialBalance)
            .Build();

        var transaction = _transactionBuilder
            .WithId(transactionId)
            .WithAmount(transactionAmount)
            .WithOperationType(operationType)
            .WithAccount(account)
            .WithAccountId(accountId)
            .Build();

        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, It.IsAny<CancellationToken>(), true, false))
            .ReturnsAsync(transaction);

        // Act
        await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.Equal(expectedBalance, account.CurrentBalance);
    }
}