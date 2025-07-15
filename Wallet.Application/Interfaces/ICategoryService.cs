using Wallet.Application.Dto.Categories;

namespace Wallet.Application.Interfaces;

public interface ICategoryService
{
    public Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);
    public Task AddAsync(CategoryDto category, CancellationToken cancellationToken);
    public Task UpdateAsync(CategoryUpdateDto category, CancellationToken cancellationToken);
    public Task DeleteAsync(int id, CancellationToken cancellationToken);
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    public Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task SeedDefaultCategoriesAsync(CancellationToken cancellationToken);
}