using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

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