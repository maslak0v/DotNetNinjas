using Microsoft.AspNetCore.Mvc;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Categories;

public class DeleteCategoryById : CategoryBase
{
    private readonly ICategoryService _categoryService;
    
    public DeleteCategoryById(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    // Добавить валидацию по всем контроллерам 
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