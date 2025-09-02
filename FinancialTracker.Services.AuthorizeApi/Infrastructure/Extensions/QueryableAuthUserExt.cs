using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Extensions
{
    public static class QueryableAuthUserExt
    {
        public static IQueryable<AuthUser> Tracking(this IQueryable<AuthUser> users, bool tracking)
            => tracking 
            ? users
            : users.AsNoTrackingWithIdentityResolution();
    }
}
