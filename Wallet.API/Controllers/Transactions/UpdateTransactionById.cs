using AutoMapper;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;
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
    public async Task<IActionResult> Update(Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Updating transaction {id}...");

        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        var response = await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);

        return await ProcessResponse(response, id, cancellationToken);
    }

    private async Task<IActionResult> ProcessResponse(OperationResult<TransactionDtoResponse> response, Guid id, CancellationToken cancellationToken)
    {
        if (!response.IsSuccess)
        {
            _logger.LogWarning($"Error updating transaction: {id}, {response.Message}");
            return ResponseCreator.Create(response);
        }

        _logger.LogInformation($"Transaction {id} updated");

        var transactionMessage = _mapper.Map<TransactionEvents.UpdateTransactionMessage>(response.Result);
        await PublishTransactionUpdate(transactionMessage, response.Result!.TransactionId, cancellationToken);

        return Ok();
    }

    private async Task PublishTransactionUpdate(IUpdateTransactionMessage transactionMessage, Guid transactionId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Publishing event [Transaction update]: update Id: {transactionId}");
        await _messagePublisher.PublishAsync(transactionMessage, cancellationToken);
    }
}