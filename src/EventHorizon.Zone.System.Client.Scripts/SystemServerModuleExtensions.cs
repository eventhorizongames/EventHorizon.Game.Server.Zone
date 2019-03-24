
using EventHorizon.Zone.System.Client.Scripts.Load;
using EventHorizon.Zone.System.Client.Scripts.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemClientScriptsExtensions
    {
        public static IServiceCollection AddSystemClientScripts(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ClientScriptRepository, ClientScriptInMemoryRepository>();
        }
        public static IApplicationBuilder UseSystemClientScripts(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(new LoadClientScriptsSystemCommand())
                    .GetAwaiter()
                    .GetResult();
            }
            return app;
        }
    }
}