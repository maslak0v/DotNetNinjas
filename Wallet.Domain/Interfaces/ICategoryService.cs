using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
    public Task AddAsync(Category category, CancellationToken cancellationToken);
    public Task UpdateAsync(Category category, CancellationToken cancellationToken);
    public Task DeleteAsync(Category category, CancellationToken cancellationToken);
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
}