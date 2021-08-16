namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerScriptsPluginEditorExtensions
    {
        public static IServiceCollection AddSystemServerScriptsPluginEditor(
            this IServiceCollection services
        ) => services
        ;

        public static IApplicationBuilder UseSystemServerScriptsPluginEditor(
            this IApplicationBuilder app
        )
        {
            using var serviceScope = app.CreateServiceScope();
            return app;
        }
    }
}
