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
    private readonly IMessagePublisher  _messagePublisher;
    
    public CreateTransaction(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger, IMessagePublisher messagePublisher) 
        : base(transactionService, mapper, logger)
    {
        _messagePublisher = messagePublisher;
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
        
        if (transactionRequest.OperationType == OperationType.Expense)
        {
            var expenseMessage = new TransactionEvents.ExpenseCreatedMessage(
                MessageId: Guid.NewGuid(),
                Timestamp: DateTime.UtcNow,
                ExpenseId: createResult.Result.TransactionId,
                UserId: createResult.Result.UserId,
                AccountId: createResult.Result.AccountId,
                CategoryName: createResult.Result.CategoryName,
                ExpenseTime: createResult.Result.TransactionDate,
                Amount: createResult.Result.Amount
            );
            
            _logger.LogInformation(
                $"Publishing event [Create expense]: created Id: {createResult.Result}");

            await _messagePublisher.PublishAsync(expenseMessage);
        }
        else
        {
            var incomeMessage = new TransactionEvents.IncomeCreatedMessage(
                MessageId: Guid.NewGuid(),
                Timestamp: DateTime.UtcNow,
                IncomeId: createResult.Result.TransactionId,
                UserId: createResult.Result.UserId,
                AccountId: createResult.Result.AccountId,
                CategoryName: createResult.Result.CategoryName,
                IncomeTime: createResult.Result.TransactionDate,
                Amount: createResult.Result.Amount
            );
            _logger.LogInformation(
                $"Publishing event [Create income]: created Id: {createResult.Result}");

            await _messagePublisher.PublishAsync(incomeMessage);
        }

        return Ok();
    }
}