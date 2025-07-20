using Microsoft.EntityFrameworkCore.Storage;
using Wallet.Application.Interfaces.Repositories;

namespace Wallet.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly WalletPostgresDbContext _context;
    private bool _disposed;

    public UnitOfWork(WalletPostgresDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        AccountRepository = new AccountRepository(_context);
        TagRepository = new TagRepository(_context);
        TransactionTagRepository = new TransactionTagRepository(_context);
        TransactionRepository = new TransactionRepository(_context);
    }

    public IAccountRepository AccountRepository { get; }
    public ITagRepository TagRepository { get; }
    public ITransactionTagRepository TransactionTagRepository { get; }
    public ITransactionRepository TransactionRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }
            _disposed = true;
        }
    }
}