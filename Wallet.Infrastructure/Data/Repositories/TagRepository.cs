using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

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

    public async Task<Tag?> GetByIdAsync(Guid tagId, CancellationToken cancellationToken)
        => await _context.Tags.FindAsync(tagId, cancellationToken);

    public async Task<IEnumerable<Tag>> GetAllUserTagsAsync(Guid userId, CancellationToken cancellationToken)
        => await _context.Tags.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<Tag>> SearchTagsByPrefixAsync(Guid userId,
        string prefix, int limit, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(t => t.UserId == userId && t.Name.StartsWith(prefix))
            .OrderBy(t => t.Name)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}