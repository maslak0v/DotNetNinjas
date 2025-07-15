using Microsoft.EntityFrameworkCore;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly WalletPostgresDbContext _context;

    public CategoryRepository(WalletPostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.CategoryId == id, cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Categories
            .AsNoTracking()
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

    public async Task ResetIdentityAsync(CancellationToken cancellationToken)
    {
        // Обновляем счетчик автоинкримента
        var maxId = await _context.Categories
            .MaxAsync(c => (int?)c.CategoryId, cancellationToken) ?? 0;
        
        var sql = $@"ALTER SEQUENCE ""Categories_CategoryId_seq"" RESTART WITH {maxId + 1}";
        await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}    