using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Extensions;

namespace Wallet.Infrastructure.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly WalletPostgresDbContext _context;

    public TransactionRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }
    
    public async Task<Transaction?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken, 
        bool includeRelated = true, 
        bool noTracking = true)
    {
        var query = _context.Transactions
            .Where(t => t.TransactionId == id && !t.IsDeleted);

        if (includeRelated)
        {
            query = query
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Include(t => t.Tag);
        }
        
        query = query.ApplyNoTracking(noTracking);

        return await query.FirstOrDefaultAsync(cancellationToken);
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
        await _context.Transactions.AddAsync(transaction, cancellationToken);
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