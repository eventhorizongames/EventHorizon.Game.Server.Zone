namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Map.Search;
    using EventHorizon.Zone.Core.Map.State;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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
                .AddSingleton<PathFindingAlgorithm, AStarSearch>()
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
        
        public static IApplicationBuilder UseCoreMap(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(
                        new CreateMap()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}