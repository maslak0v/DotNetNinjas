using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Categories;

public class DeleteCategoryById : CategoryBase
{
    public DeleteCategoryById(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);

        if (category != null)
        {
            await _categoryService.DeleteAsync(category, cancellationToken);
            return NoContent();
        }
        
        return NotFound();
    }
}