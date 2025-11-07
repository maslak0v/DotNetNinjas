
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using Wallet.Application.Interfaces.Repositories;

namespace Wallet.Infrastructure.Messaging
{
    public class UserDeletedWalletConsumer(
        ILogger<UserDeletedWalletConsumer> logger,
        IAccountRepository accountRepository) : IConsumer<IUserDeleted>
    {
        public async Task Consume(ConsumeContext<IUserDeleted> context)
        {
            var msg = context.Message;
            logger.LogInformation("[RabbitMQ] User deleted: {userId}", msg.UserId);
            await accountRepository.SoftDeleteRangeByUserIdAsync(msg.UserId, msg.Timestamp, context.CancellationToken);
        }
    }
}
