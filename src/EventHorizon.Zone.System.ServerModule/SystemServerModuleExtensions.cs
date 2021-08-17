
namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.ServerModule.Load;
    using EventHorizon.Zone.System.ServerModule.State;

    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerModuleExtensions
    {
        public static IServiceCollection AddSystemServerModule(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ServerModuleRepository, ServerModuleInMemoryRepository>()
            ;
        }

        public static IApplicationBuilder UseSystemServerModule(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetRequiredService<IMediator>()
                    .Publish(new LoadServerModuleSystem())
                    .GetAwaiter()
                    .GetResult();
                return app;
            }
        }
    }
}
