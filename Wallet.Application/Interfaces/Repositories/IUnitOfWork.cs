namespace Wallet.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    ITagRepository TagRepository { get; }
    ITransactionTagRepository TransactionTagRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}