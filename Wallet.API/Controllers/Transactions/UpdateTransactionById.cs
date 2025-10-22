using AutoMapper;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class UpdateTransactionById : TransactionBase<UpdateTransactionById>
{
    private readonly IMessagePublisher _messagePublisher;

    public UpdateTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<UpdateTransactionById> logger,
        IMessagePublisher messagePublisher) 
        : base(transactionService, mapper, logger)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Delete( Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Обновление транзакции {id}...");
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        var response = await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);
        
        if (!response.IsSuccess)
        {
            _logger.LogWarning($"Ошибка обновления транзакции: {id}, {response.Message}");
            return ResponseCreator.Create(response);
        }

        _logger.LogInformation($"Транзакции {id}, обновлена");
        
        IUpdateTransactionMessage transactionMessage = _mapper.Map<TransactionEvents.UpdateTransactionMessage>(response.Result);
        
        _logger.LogInformation(
                $"Publishing event [Transaction update]: update Id: {response.Result!.TransactionId}");

        await _messagePublisher.PublishAsync(transactionMessage, cancellationToken);

        return Ok();
    }
}