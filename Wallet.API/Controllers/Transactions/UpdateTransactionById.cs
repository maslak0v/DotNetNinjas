using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Transactions;

public class UpdateTransactionById : TransactionBase
{   
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;

    public UpdateTransactionById(ITransactionService transactionService, IMapper mapper)
    {
        _transactionService = transactionService;
        _mapper = mapper;
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Create( Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);
        
        return NoContent();
    }
}