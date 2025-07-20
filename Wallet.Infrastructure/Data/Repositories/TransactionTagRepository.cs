using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Repositories;

public class TransactionTagRepository : ITransactionTagRepository
{
    private readonly WalletPostgresDbContext _context;

    public TransactionTagRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }

    public async Task<TransactionTag?> GetByTransactionIdAsync(
        Guid id, 
        CancellationToken cancellationToken, 
        bool noTracking = true)
    {
        var query = _context.TransactionTags
            .Where(tt => tt.TransactionId == id);
        
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(TransactionTag transactionTag, CancellationToken cancellationToken)
    {
        await _context.TransactionTags.AddAsync(transactionTag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(Guid transactionTagId, CancellationToken cancellationToken)
    {
        var entity = await _context.TransactionTags.FindAsync(new object[] { transactionTagId }, cancellationToken);
        if (entity != null)
        {
            _context.TransactionTags.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task UpdateAsync(TransactionTag transactionTag, CancellationToken cancellationToken)
    {
        _context.TransactionTags.Update(transactionTag);
        await _context.SaveChangesAsync(cancellationToken);
    }
}