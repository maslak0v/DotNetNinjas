
using Primitives.Shared.Exceptions;

namespace Primitives.Shared.Extensions
{
    public static class CollectionExt
    {
        public static void ThrowIfEmpty<T>(this ICollection<T> collection)
        {
            if (collection is null || collection.Count == 0)

                throw new NotFoundException("Null or empty collection");
        }
    }
}
