using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.State;
using EventHorizon.Game.Server.Zone.Admin.Server.State;
using EventHorizon.Game.Server.Zone.Server.Api;
using EventHorizon.Game.Server.Zone.Server.Load;
using EventHorizon.Game.Server.Zone.Server.Model;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class ServerExtensions
    {
        public static IServiceCollection AddSystemServer(this IServiceCollection services)
        {
            return services
                .AddSingleton<ServerScriptRepository, ServerScriptInMemoryRepository>()
                .AddTransient<IServerScriptServices, ServerScriptServices>()
            ;
        }
        public static IApplicationBuilder UseSystemServer(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadServerScripts())
                    .GetAwaiter().GetResult();
            }
            return app;
        }
    }
}