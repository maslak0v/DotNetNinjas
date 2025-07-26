using AutoMapper;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class DeleteTransaction : TransactionBase<DeleteTransaction>
{
    private readonly IMessagePublisher  _messagePublisher;
    
    public DeleteTransaction(
        ITransactionService transactionService,
        IMapper mapper,
        ILogger<DeleteTransaction> logger, IMessagePublisher messagePublisher)
        : base(transactionService, mapper, logger)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Удаление транзакции {id}...");
    
        var resultOperation = await _transactionService.DeleteAsync(id, cancellationToken);

        if (resultOperation.IsSuccess)
        {
            var transactionDeletedMessage = _mapper.Map<TransactionEvents.TransactionDeletedMessage>(resultOperation.Result);
            
            _logger.LogInformation(
                $"Publishing event [{nameof(IDeleteTransactionMessage)}]: " +
                $"transaction [{id}] deleted");
            
            await _messagePublisher.PublishAsync(transactionDeletedMessage);
        }    
    
        _logger.LogInformation($"Транзакция {id} удалена");
        return Ok();
    }
}