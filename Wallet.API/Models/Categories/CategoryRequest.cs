using System.ComponentModel.DataAnnotations;

namespace Wallet.API.Models.Categories;

public class CategoryRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Icon { get; set; } = "default";
}