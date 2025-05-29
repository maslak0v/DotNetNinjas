using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using System.Security.Cryptography;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveAsync();
        void Add(RefreshToken token);
        void Revoke(string jti);
        Task<RefreshToken?> FindAsync(string jti);
    }
}
