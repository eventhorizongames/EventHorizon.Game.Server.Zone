using EventHorizon.Zone.Core.Map.State;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class CoreMapExtensions
    {
        public static IServiceCollection AddCoreMap(
            this IServiceCollection services
        )
        {
            var serverMap = new InMemoryServerMap();
            return services
                .AddSingleton<IServerMap>(
                    serverMap
                )
                .AddTransient<IMapGraph>(
                    _ => serverMap.Map()
                )
                .AddTransient<IMapDetails>(
                    _ => serverMap.MapDetails()
                )
                .AddTransient<IMapMesh>(
                    _ => serverMap.MapMesh()
                )
            ;
        }
        // public static IApplicationBuilder UseCoreMap(
        //     this IApplicationBuilder app
        // )
        // {
        //     using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //     {
        //         var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
        //         mediator.Send(
        //             new LoadZoneAgentStateEvent()
        //         ).GetAwaiter().GetResult();

        //         return app;
        //     }
        // }
    }
}