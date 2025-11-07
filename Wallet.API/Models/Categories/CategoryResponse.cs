namespace Wallet.API.Models.Categories;

public class CategoryResponse
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
}