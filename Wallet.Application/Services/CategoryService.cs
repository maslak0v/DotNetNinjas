using Wallet.Application.Interfaces;
using Wallet.Domain.Defaults;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Application.Services;

public class CategoryService: ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken) 
        => await _categoryRepository.GetByIdAsync(id, cancellationToken);
    public async Task SeedDefaultCategoriesAsync(CancellationToken cancellationToken)
    {
        foreach (var defaultCategory in DefaultCategories.List)
        {
            var existingCategory =
                await _categoryRepository.GetByIdAsync(defaultCategory.CategoryId, cancellationToken);
            if (existingCategory == null)
            {
                await _categoryRepository.AddAsync(defaultCategory, cancellationToken);
            }
            else
            {
                bool isUpdated = false;
                if (existingCategory.Name != defaultCategory.Name)
                {
                    existingCategory.Name = defaultCategory.Name;
                    isUpdated = true;
                }

                if (existingCategory.Icon != defaultCategory.Icon)
                {
                    existingCategory.Icon = defaultCategory.Icon;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await _categoryRepository.UpdateAsync(existingCategory, cancellationToken);
                }
            }
        }
    }
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken) 
        => await _categoryRepository.GetAllAsync(cancellationToken);

    public async Task AddAsync(Category category, CancellationToken cancellationToken) 
        => await _categoryRepository.AddAsync(category, cancellationToken);

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken) 
        => await _categoryRepository.UpdateAsync(category, cancellationToken); 

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken) 
        => await _categoryRepository.DeleteAsync(category, cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) 
        => await _categoryRepository.ExistsAsync(id, cancellationToken);

    public async Task<Category?> GetById(int id, CancellationToken cancellationToken)
        => await _categoryRepository.GetByIdAsync(id, cancellationToken);
}