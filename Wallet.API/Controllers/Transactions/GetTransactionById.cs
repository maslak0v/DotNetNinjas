using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.API.Models.Transactions;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class GetTransactionById : TransactionBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;
    
    public GetTransactionById(IMapper mapper, ITransactionService transactionService)
    {
        _mapper = mapper;
        _transactionService = transactionService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);
        var response = _mapper.Map<TransactionResponse>(transaction);
        return Ok(response);
    }
}