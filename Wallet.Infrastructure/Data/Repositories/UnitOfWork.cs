using Microsoft.EntityFrameworkCore.Storage;
using Wallet.Application.Interfaces.Repositories;

namespace Wallet.Infrastructure.Data.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly WalletPostgresDbContext _context;
    private IDbContextTransaction? _currentTransaction;
    public UnitOfWork(
        WalletPostgresDbContext context,
        IAccountRepository accountRepository,
        ITagRepository tagRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository)
    {
        _context = context;
        AccountRepository = accountRepository;
        TagRepository = tagRepository;
        TransactionRepository = transactionRepository;
        CategoryRepository = categoryRepository;
    }

    public IAccountRepository AccountRepository { get; }
    public ITagRepository TagRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public ICategoryRepository CategoryRepository { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => _context.SaveChangesAsync(cancellationToken);
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Транзакция уже начата. Завершите её перед началом новой.");

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("Нет активной транзакции для фиксации.");

        try
        {
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}