using System.ComponentModel.DataAnnotations;

namespace Wallet.API.Models.Tags;

public class TagRequest
{
    [StringLength(50)] 
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }
 }    
    