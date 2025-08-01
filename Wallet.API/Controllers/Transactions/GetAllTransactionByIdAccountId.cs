using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Helpers;
using Wallet.API.Models.Transactions;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;

public class GetAllTransactionByIdAccountId : TransactionBase<GetAllTransactionByIdAccountId>
{
    public GetAllTransactionByIdAccountId(
        ITransactionService transactionService, 
        IMapper mapper, 
        ILogger<GetAllTransactionByIdAccountId> logger) 
        : base(transactionService, mapper, logger)
    {
    }

    [HttpGet("all/{accountId:guid}")]
    public async Task<IActionResult> GetById(Guid accountId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Получение транзакций для аккаунта {accountId}...");
    
        var resultOperation = await _transactionService.GetByAccountIdAsync(accountId, cancellationToken);
    
        if (!resultOperation.IsSuccess)
        {
            return ResponseCreator.Create(resultOperation);
        }
        
        var mappedResult = _mapper.Map<IEnumerable<TransactionResponse>>(resultOperation.Result);
    
        return ResponseCreator.Create(OperationResult<IEnumerable<TransactionResponse>>.Success(mappedResult));
    }
}