using Microsoft.EntityFrameworkCore;

namespace Wallet.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyNoTracking<T>(this IQueryable<T> query, bool noTracking) where T : class
    {
        return noTracking ? query.AsNoTracking() : query;
    }
}