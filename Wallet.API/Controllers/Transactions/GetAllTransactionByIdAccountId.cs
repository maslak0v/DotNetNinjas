using AutoMapper;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class GetAllTransactionByIdAccountId : TransactionBase
{
    public GetAllTransactionByIdAccountId(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger) 
        : base(transactionService, mapper, logger)
    {
    }

    [HttpGet("all/{accountId:guid}")]
    public async Task<IActionResult> GetById(Guid accountId, CancellationToken cancellationToken)
    {
        var transactions = await _transactionService.GetByAccountIdAsync(accountId, cancellationToken);
        var response = _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        return Ok(response);
    }
}