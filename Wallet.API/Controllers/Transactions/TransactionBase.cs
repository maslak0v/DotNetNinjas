using AutoMapper;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Transactions;
[ApiController]
[Route("api/account/transaction")]
[ApiExplorerSettings(GroupName = "Управление транзакциями")]
public class TransactionBase : ControllerBase
{
    protected readonly ITransactionService _transactionService;
    protected readonly IMapper _mapper;
    protected readonly ILogger<TransactionBase> _logger;
    protected readonly IMessagePublisher messagePublisher;

    public TransactionBase(ITransactionService transactionService, IMapper mapper, ILogger<TransactionBase> logger, IMessagePublisher messagePublisher)
    {
        _transactionService = transactionService;
        _mapper = mapper;
        _logger = logger;
        this.messagePublisher = messagePublisher;
    }
}