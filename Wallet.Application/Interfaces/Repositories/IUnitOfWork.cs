namespace Wallet.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    ITagRepository TagRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}