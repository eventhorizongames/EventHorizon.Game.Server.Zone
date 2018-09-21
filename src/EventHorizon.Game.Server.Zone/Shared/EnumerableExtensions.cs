
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.Server.Zone.ServerAction.Timer;
using EventHorizon.TimerService;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Shared
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