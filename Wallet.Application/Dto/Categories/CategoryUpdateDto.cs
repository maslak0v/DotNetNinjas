namespace Wallet.Application.Dto.Categories;

public class CategoryUpdateDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
}
