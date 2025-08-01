using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Categories;

public class DeleteCategoryById : CategoryBase
{
    public DeleteCategoryById(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}