using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class CreateTransaction: TransactionBase
{
    public CreateTransaction(ITransactionService transactionService, IMapper mapper) : base(transactionService, mapper)
    {
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        return Ok(await _transactionService.CreateAsync(transactionDto, cancellationToken));
    }
}