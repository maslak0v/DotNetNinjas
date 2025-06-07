using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class DeleteTransaction : TransactionBase
{
    private readonly ITransactionService _transactionService;
  

    public DeleteTransaction(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Create( Guid id, CancellationToken cancellationToken)
    {
        await _transactionService.DeleteAsync(id, cancellationToken);
        
        return Ok();
    }
}