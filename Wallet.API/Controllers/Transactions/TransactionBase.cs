using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;
[ApiController]
[Route("api/account/transaction")]
[ApiExplorerSettings(GroupName = "Управление транзакциями")]
public abstract class TransactionBase<T> : ControllerBase where T : TransactionBase<T>
{
    protected readonly ITransactionService _transactionService;
    protected readonly IMapper _mapper;
    protected readonly ILogger<T> _logger;
    
    protected TransactionBase(ITransactionService transactionService, IMapper mapper, ILogger<T> logger)
    {
        _transactionService = transactionService;
        _mapper = mapper;
        _logger = logger;
    }
}