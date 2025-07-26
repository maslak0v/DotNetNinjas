using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class GetTransactionById : TransactionBase<GetTransactionById>
{
    public GetTransactionById(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<GetTransactionById> logger) 
        : base(transactionService, mapper, logger)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Получение транзакции {id}");

        var resultOperation = await _transactionService.GetByIdAsync(id, cancellationToken);
    
        if (!resultOperation.IsSuccess)
        {
            return ResponseCreator.Create(resultOperation);
        }
        
        var response = _mapper.Map<TransactionResponse>(resultOperation.Result);
        return ResponseCreator.Create(OperationResult<TransactionResponse>.Success(response));
    }
}