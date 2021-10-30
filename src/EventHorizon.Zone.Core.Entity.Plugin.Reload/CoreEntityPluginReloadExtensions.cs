namespace EventHorizon.Zone.Core.Entity.Plugin.Reload
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreEntityPluginReloadExtensions
    {
        public static IServiceCollection AddCoreEntityPluginReload(
            this IServiceCollection services
        ) => services
        ;

        public static IApplicationBuilder UseCoreEntityPluginReload(
            this IApplicationBuilder app
        ) => app;

    }
}
