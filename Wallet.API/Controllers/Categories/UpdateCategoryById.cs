using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Categories;

public class UpdateCategoryById : CategoryBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public UpdateCategoryById(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
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