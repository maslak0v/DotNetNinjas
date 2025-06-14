using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class GetAllTransactionByIdAccountId : TransactionBase
{
    public GetAllTransactionByIdAccountId(ITransactionService transactionService, IMapper mapper) : base(transactionService, mapper)
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