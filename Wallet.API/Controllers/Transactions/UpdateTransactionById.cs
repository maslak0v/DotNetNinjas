using AutoMapper;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class UpdateTransactionById : TransactionBase
{
    public UpdateTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger, 
        IMessagePublisher messagePublisher) 
        : base(transactionService, mapper, logger, messagePublisher)
    {
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Create( Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);
        
        return NoContent();
    }
}