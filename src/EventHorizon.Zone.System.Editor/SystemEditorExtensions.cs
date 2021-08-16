namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemEditorExtensions
    {
        public static IServiceCollection AddSystemEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemEditor(
            this IApplicationBuilder app
        )
        {
            using var serviceScope = app.CreateServiceScope();
            return app;
        }
    }
}
