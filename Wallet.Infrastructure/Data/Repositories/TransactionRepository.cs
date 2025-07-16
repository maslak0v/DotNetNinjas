using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly WalletPostgresDbContext _context;

    public TransactionRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == id && !t.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == accountId && !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        await _context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken); 
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.TransactionId == id && !t.IsDeleted, cancellationToken);

        if (transaction == null)
            throw new KeyNotFoundException($"Не найдена транзакция");

        transaction.IsDeleted = true;
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AnyAsync(t => t.TransactionId == id && !t.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate, int limit, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == accountId && !t.IsDeleted && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .OrderByDescending(t => t.TransactionDate )
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}