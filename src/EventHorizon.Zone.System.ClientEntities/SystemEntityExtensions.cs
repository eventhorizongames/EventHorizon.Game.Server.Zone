
using EventHorizon.Zone.System.ClientEntities.Load;
using EventHorizon.Zone.System.ClientEntities.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemClientEntitiesExtensions
    {
        public static IServiceCollection AddSystemClientEntities(this IServiceCollection services)
        {
            return services
                .AddSingleton<ClientEntityInstanceRepository, ClientEntityInstanceInMemoryRepository>();
        }
        public static void UseSystemClientEntities(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadSystemClientEntitiesCommand())
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}