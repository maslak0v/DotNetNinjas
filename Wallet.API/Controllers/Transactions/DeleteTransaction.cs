using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class DeleteTransaction : TransactionBase
{
    public DeleteTransaction(ITransactionService transactionService, IMapper mapper) : base(transactionService, mapper)
    {
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Create( Guid id, CancellationToken cancellationToken)
    {
        await _transactionService.DeleteAsync(id, cancellationToken);
        
        return Ok();
    }
}