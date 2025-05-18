using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface ITagRepository
{
    Task AddAsync(Tag tag);
    Task<IEnumerable<Tag>> SearchAsync(string searchTerm, Guid userId, int limit = 5);
}