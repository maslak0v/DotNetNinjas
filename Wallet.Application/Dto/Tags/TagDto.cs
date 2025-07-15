namespace Wallet.Application.Dto.Tags;

public class TagDto
{
    public Guid TagId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}