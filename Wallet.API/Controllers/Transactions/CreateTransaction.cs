using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class CreateTransaction: TransactionBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;
    
    public CreateTransaction(IMapper mapper, ITransactionService transactionService)
    {
        _mapper = mapper;
        _transactionService = transactionService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        return Ok(await _transactionService.CreateAsync(transactionDto, cancellationToken));
    }
}