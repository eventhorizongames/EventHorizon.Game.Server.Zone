using EventHorizon.Game.Server.Zone.Plugin.Load;
using EventHorizon.Game.Server.Zone.Plugin.State;
using EventHorizon.Game.Server.Zone.Plugin.State.Loader;
using EventHorizon.Game.Server.Zone.Plugin.State.Server;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Plugin
{
    public static class PluginExtensions
    {
        public static IServiceCollection AddPlugins(
            this IServiceCollection services,
            IHostingEnvironment hostingEnvironment)
        {
            return services
                // Server Plugin State needs to be done as part of Service startup to load plugin Assembly files into the DI Container.
                .AddSingleton<PluginState>(
                    new ServerPluginState(
                        hostingEnvironment,
                        services,
                        new ServerPluginLoader(
                            hostingEnvironment.IsDevelopment())));
        }
        public static IApplicationBuilder UsePlugins(
            this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Publish(
                    new StartupPluginsEvent()
                ).GetAwaiter().GetResult();
            }

            return app;
        }
    }
}