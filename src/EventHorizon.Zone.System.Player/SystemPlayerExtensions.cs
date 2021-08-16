namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemPlayerExtensions
    {
        public static IServiceCollection AddSystemPlayer(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemPlayer(
            this IApplicationBuilder app
        )
        {
            using var serviceScope = app.CreateServiceScope();
            return app;
        }
    }
}
