namespace System.Collections.Generic;

using System.Linq;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Values<T>(
        this IEnumerable<T> source
    )
    {
        return source ?? Enumerable.Empty<T>();
    }
}
