namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreClientExtensions
    {
        public static IServiceCollection AddCoreClient(
            this IServiceCollection services
        ) => services;
    }
}
