
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.State;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemEntityModuleExtensions
    {
        public static IServiceCollection AddSystemEntityModule(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<EntityModuleRepository, EntityModuleInMemoryRepository>()
            ;
        }
        public static void UseSystemEntityModule(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(new LoadEntityModuleSystemCommand())
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
