using Microsoft.EntityFrameworkCore.Storage;

namespace Wallet.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    // Основные репозитории
    IAccountRepository AccountRepository { get; }
    ITagRepository TagRepository { get; }
    ITransactionTagRepository TransactionTagRepository { get; }
    ITransactionRepository TransactionRepository { get; }

    // Сохранение изменений
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}