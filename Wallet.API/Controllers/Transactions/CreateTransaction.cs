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

public class CreateTransaction: TransactionBase<CreateTransaction>
{
    private readonly IMessagePublisher  _messagePublisher;
    
    public CreateTransaction(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<CreateTransaction> logger, IMessagePublisher messagePublisher) 
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
            var expenseMessage = _mapper.Map<TransactionEvents.ExpenseCreatedMessage>(createResult.Result);
            
            _logger.LogInformation(
                $"Publishing event [Create expense]: created Id: {createResult.Result!.TransactionId}");
            
            await _messagePublisher.PublishAsync(expenseMessage);
        }
        else
        {
            var incomeMessage = _mapper.Map<TransactionEvents.IncomeCreatedMessage>(createResult.Result);
            
            _logger.LogInformation(
                $"Publishing event [Create income]: created Id: {createResult.Result!.TransactionId}");

            await _messagePublisher.PublishAsync(incomeMessage);
        }
        
        _logger.LogInformation($"Создана транзакция {createResult.Result!.TransactionId}");
        return Ok();
    }
}