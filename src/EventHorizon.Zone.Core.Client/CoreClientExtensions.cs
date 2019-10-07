using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class CoreClientExtensions
    {
        public static IServiceCollection AddCoreClient(
            this IServiceCollection services
        ) => services;
    }
}