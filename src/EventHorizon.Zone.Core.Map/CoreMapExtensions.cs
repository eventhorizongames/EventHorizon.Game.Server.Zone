namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Map.Search;
    using EventHorizon.Zone.Core.Map.State;

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
                .AddTransient(
                    _ => serverMap.Map()
                )
                .AddTransient(
                    _ => serverMap.MapDetails()
                )
                .AddTransient(
                    _ => serverMap.MapMesh()
                )
            ;
        }

        public static IApplicationBuilder UseCoreMap(
            this IApplicationBuilder app
        ) => app.SendMediatorCommand(
            new CreateMap()
        );
    }
}
