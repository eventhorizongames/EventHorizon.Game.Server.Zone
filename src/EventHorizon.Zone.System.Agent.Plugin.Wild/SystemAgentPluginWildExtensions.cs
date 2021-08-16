namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAgentPluginWildExtensions
    {
        public static IServiceCollection AddSystemAgentPluginWild(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemAgentPluginWild(
            this IApplicationBuilder app
        ) => app;
    }
}
