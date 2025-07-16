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

    public async Task<TransactionTag> GetByTransactionIdAsync(Guid id, CancellationToken cancellationToken) 
        => await _context.TransactionTags
             .AsNoTracking()
             .FirstOrDefaultAsync(t => t.TransactionId == id, cancellationToken);

    public async Task AddAsync(TransactionTag transactionTag, CancellationToken cancellationToken)
    {
        await _context.TransactionTags.AddAsync(transactionTag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(TransactionTag transactionTag, CancellationToken cancellationToken)
    {
        await _context.TransactionTags.AddAsync(transactionTag, cancellationToken); 
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TransactionTag transactionTag, CancellationToken cancellationToken)
    {
        _context.TransactionTags.Update(transactionTag);
        await _context.SaveChangesAsync(cancellationToken);
    }
}