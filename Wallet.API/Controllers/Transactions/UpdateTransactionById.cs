using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class UpdateTransactionById : TransactionBase
{
    public UpdateTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<TransactionBase> logger) 
        : base(transactionService, mapper, logger)
    {
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Create( Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        var response = await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);
        
        if (!response.IsSuccess)
        {
            _logger.LogWarning($"Ошибка создания транзакции: {response.Message}");
            return ResponseCreator.Create(response);
        } 
        
        return Ok();
    }
}