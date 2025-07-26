using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class UpdateTransactionById : TransactionBase<UpdateTransactionById>
{
    public UpdateTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<UpdateTransactionById> logger) 
        : base(transactionService, mapper, logger)
    {
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Delete( Guid id, [FromBody] TransactionRequest transactionRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Обновление транзакции {id}...");
        var transactionDto = _mapper.Map<TransactionDto>(transactionRequest);
        var response = await _transactionService.UpdateAsync(id, transactionDto, cancellationToken);
        
        if (!response.IsSuccess)
        {
            _logger.LogWarning($"Ошибка обновления транзакции: {id}, {response.Message}");
            return ResponseCreator.Create(response);
        } 
       
        _logger.LogInformation($"Транзакции {id}, обновлена");
        return Ok();
    }
}