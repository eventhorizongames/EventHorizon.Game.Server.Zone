namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAgentPluginAiExtensions
    {
        public static IServiceCollection AddSystemAgentPluginAi(
            this IServiceCollection services
        )
        {
            return services
            ;
        }
        public static IApplicationBuilder UseSystemAgentPluginAi(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}
