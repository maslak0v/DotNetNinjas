using AutoMapper;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class GetTransactionById : TransactionBase
{
    public GetTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger, 
        IMessagePublisher messagePublisher) 
        : base(transactionService, mapper, logger, messagePublisher)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);
        var response = _mapper.Map<TransactionResponse>(transaction);
        return Ok(response);
    }
}