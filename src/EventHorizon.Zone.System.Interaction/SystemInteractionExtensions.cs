namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemInteractionExtensions
    {
        public static IServiceCollection AddSystemInteraction(
            this IServiceCollection services
        )
        {
            return services;
        }

        public static IApplicationBuilder UseSystemInteraction(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}