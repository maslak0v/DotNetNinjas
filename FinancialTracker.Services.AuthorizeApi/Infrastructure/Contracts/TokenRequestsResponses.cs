using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts
{
    public record RefreshRequest(
        [Required(ErrorMessage = "Refresh token is required.")]
        [MinLength(1, ErrorMessage = "Refresh token cannot be empty.")]
        string RefreshToken,

        Guid Jti): IRefreshRequest;
}
