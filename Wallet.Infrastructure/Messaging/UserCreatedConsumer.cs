using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Infrastructure.Messaging;

public class UserCreatedConsumer : IConsumer<IUserCreated>
{
    private readonly ILogger<UserCreatedConsumer> _logger;
    private readonly IAccountRepository _accountRepository;
    
    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    public async Task Consume(ConsumeContext<IUserCreated> context)
    {
        var msg = context.Message;
        _logger.LogInformation($"[RabbitMQ] User created: {msg.UserId}");
       
        var user = new Account
        {
            UserId = msg.UserId, 
            Name = "Ваш первый счет",
            CurrentBalance = 0,
            Currency = Currency.RUB
        };
        
        await _accountRepository.AddAsync(user, CancellationToken.None);
    }
}