using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Application.Dto.Categories;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Categories;

public class CreateCategory : CategoryBase
{
    public CreateCategory(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<CategoryDto>(categoryRequest);
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = null;
        await _categoryService.AddAsync(category, cancellationToken);
        return NoContent();
    }
}