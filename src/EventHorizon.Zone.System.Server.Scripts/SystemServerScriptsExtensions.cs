using EventHorizon.Game.Server.Zone.Admin.Server.State;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemServerScriptsExtensions
    {
        public static IServiceCollection AddSystemServerScripts(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ServerScriptRepository, ServerScriptInMemoryRepository>()
                .AddTransient<ServerScriptServices, SystemServerScriptServices>()
            ;
        }
        public static IApplicationBuilder UseSystemServerScripts(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadServerScriptsCommand())
                    .GetAwaiter().GetResult();
            }
            return app;
        }
    }
}