using AutoMapper;
using Wallet.Application.Dto.Categories;
using Wallet.Application.Interfaces;
using Wallet.Domain.Defaults;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Application.Services;

public class CategoryService: ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await  _categoryRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CategoryDto>(category);
    }
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
        
        await _categoryRepository.ResetIdentityAsync(cancellationToken);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task AddAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        await _categoryRepository.AddAsync(categoryEntity, cancellationToken);
    }

    public async Task UpdateAsync(CategoryUpdateDto category, CancellationToken cancellationToken)
    {
        var categoryEntity = _mapper.Map<Category>(category);
        await _categoryRepository.UpdateAsync(categoryEntity, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        
        if (category == null)
        {
            throw new InvalidOperationException($"Категория с id {id} не найдена.");
        }
        await _categoryRepository.DeleteAsync(category, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) 
        => await _categoryRepository.ExistsAsync(id, cancellationToken);

    public async Task<CategoryDto?> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CategoryDto>(category);
    }
}