using AutoMapper;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Interfaces.Services;
using Wallet.Domain.Enums;

namespace Wallet.API.Controllers.Transactions;

public class CreateTransaction: TransactionBase
{
    public CreateTransaction(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger, 
        IMessagePublisher messagePublisher) 
        : base(transactionService, mapper, logger, messagePublisher)
    {
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Создание транзакции {transactionRequest}...");
        
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        
        var createResult = await _transactionService.CreateAsync(transactionDto, cancellationToken);
        if (!createResult.IsSuccess)
        {
            _logger.LogWarning($"Ошибка создания транзакции: {createResult.Message}");
            return ResponseCreator.Create(createResult);
        }
        
        var transactionId = Guid.Parse(createResult.Result.ToString());
        var transaction = await _transactionService.GetByIdAsync(transactionId , cancellationToken);
        
        if (transactionRequest.OperationType == OperationType.Expense)
        {
            var expenseMessage = new TransactionEvents.ExpenseCreatedMessage(
                MessageId: Guid.NewGuid(),
                Timestamp: DateTime.UtcNow,
                ExpenseId: transaction.TransactionId,
                UserId: transaction.UserId,
                AccountId: transaction.AccountId,
                CategoryName: transaction.CategoryName,
                ExpenseTime: transaction.TransactionDate,
                Amount: transaction.Amount
            );
            
            _logger.LogInformation(
                $"Publishing event [Create expense]: created Id: {transactionId}");

            await messagePublisher.PublishAsync(expenseMessage);
        }
        else
        {
            var incomeMessage = new TransactionEvents.IncomeCreatedMessage(
                MessageId: Guid.NewGuid(),
                Timestamp: DateTime.UtcNow,
                IncomeId: transaction.TransactionId,
                UserId: transaction.UserId,
                AccountId: transaction.AccountId,
                CategoryName: transaction.CategoryName,
                IncomeTime: transaction.TransactionDate,
                Amount: transaction.Amount
            );
            _logger.LogInformation(
                $"Publishing event [Create income]: created Id: {transactionId}");

            await messagePublisher.PublishAsync(incomeMessage);
        }
        
        return ResponseCreator.Create<Guid>(createResult);
    }
}