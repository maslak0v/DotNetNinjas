using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Repositories;

public class TagRepository : ITagRepository
{
    private readonly WalletPostgresDbContext _context;

    public TagRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        await _context.Tags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<Tag?> GetUserTagByNameAsync(string name, Guid userId, CancellationToken cancellationToken) 
        => await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name && t.UserId == userId, cancellationToken);

    public async Task<IEnumerable<Tag>> SearchUserTagsByPrefixAsync(Guid userId, string prefix, int limit, int page, CancellationToken cancellationToken)
    {
        int skip = (page - 1) * limit;

        var query = _context.Tags
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(prefix))
        {
            query = query.Where(t => t.Name.StartsWith(prefix));
        }
        
        return await query
            .OrderBy(t => t.Name)
            .Skip(skip)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}