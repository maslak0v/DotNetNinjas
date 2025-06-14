using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.API.Controllers.Categories;

public class UpdateCategoryById : CategoryBase
{
    public UpdateCategoryById(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(categoryRequest);
        category.CategoryId = id; 

        await _categoryService.UpdateAsync(category, cancellationToken);

        return NoContent();
    }
}