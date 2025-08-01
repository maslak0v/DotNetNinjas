using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Extensions;

namespace Wallet.Infrastructure.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly WalletPostgresDbContext _context;

    public AccountRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }


    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool noTracking = true)
    {
        var query = _context.Account
            .ApplyNoTracking(noTracking)
            .Where(a => a.AccountId == id);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid guid, CancellationToken cancellationToken)
    {
        return await _context.Account
            .AsNoTracking()
            .Where(a => a.UserId == guid)
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddAsync(Account account, CancellationToken cancellationToken)
    {
       await _context.AddAsync(account, cancellationToken);
       await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        _context.Update(account);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDelete(Account account, CancellationToken cancellationToken)
    {
        account.IsDeleted = true;
        account.UpdatedAt = DateTime.UtcNow;

        _context.Account.Update(account);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}