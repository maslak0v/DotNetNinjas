using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

namespace Wallet.Infrastructure.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WalletPostgresDbContext _context;
    
    public CategoryRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }
    
    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken) 
        => await _context.Categories.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken) 
        => await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) 
        => await _context.Categories
            .AnyAsync(c => c.CategoryId == id, cancellationToken);
    
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}