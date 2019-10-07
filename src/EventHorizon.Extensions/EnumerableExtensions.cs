
using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Values<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source?.Any() ?? true;
        }
    }
}