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

public class TransactionServiceGetTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TransactionService _transactionService;
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionServiceGetTests()
    {
        _transactionBuilder = new TransactionBuilder();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        
        _transactionService = new TransactionService(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTransaction()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        
        var transaction = _transactionBuilder
            .WithAccountId(accountId)
            .Build();
        
        var transactionDto = new TransactionDtoResponse 
        { 
            TransactionId = transactionId,
            AccountId = accountId,
            Amount = 100,
            OperationType = OperationType.Income
        };
        
        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, 
                It.IsAny<CancellationToken>(), 
                true, 
                true))
            .ReturnsAsync(transaction);
            
        _mapperMock.Setup(x => x.Map<TransactionDtoResponse>(transaction))
            .Returns(transactionDto);

        // Act
        var result = await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(transactionDto, result.Result);
        Assert.Equal("Ok", result.Message);
        Assert.Equal(transactionId, result.Result!.TransactionId);
        Assert.Equal(accountId, result.Result.AccountId);
    }

    [Fact]
    public async Task GetByIdAsync_WithRelatedData_ReturnsCompleteTransaction()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        
        var transaction = _transactionBuilder
            .WithAccountId(accountId)
            .WithTagId(tagId)
            .WithOperationType(OperationType.Expense)
            .WithAmount(500)
            .Build();
        
        var transactionDto = new TransactionDtoResponse 
        { 
            TransactionId = transactionId,
            AccountId = accountId,
            Amount = 500,
            OperationType = OperationType.Expense,
            TagId = tagId
        };
        
        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, 
                It.IsAny<CancellationToken>(), 
                true, 
                true))
            .ReturnsAsync(transaction);
            
        _mapperMock.Setup(x => x.Map<TransactionDtoResponse>(transaction))
            .Returns(transactionDto);

        // Act
        var result = await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OperationType.Expense, result.Result!.OperationType);
        Assert.Equal(500, result.Result.Amount);
        Assert.Equal(tagId, result.Result.TagId);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        
        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, 
                It.IsAny<CancellationToken>(), 
                true, 
                true))
            .ReturnsAsync((Transaction)null!);

        // Act
        var result = await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Enum_StatusCode.NotFound, result.StatusCode);
        Assert.Equal($"Транзакция не найдена: {transactionId}", result.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryThrowsException_ReturnsErrorResult()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var exception = new Exception("Ошибка базы данных");
    
        _unitOfWorkMock.Setup(x => x.TransactionRepository.GetByIdAsync(
                transactionId, 
                It.IsAny<CancellationToken>(), 
                true, 
                true))
            .ThrowsAsync(exception);

        // Act
        var result = await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(exception.Message, result.Message!.Trim());
    }
}