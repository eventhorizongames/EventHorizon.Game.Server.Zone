
using EventHorizon.Zone.System.ServerModule.Load;
using EventHorizon.Zone.System.ServerModule.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemServerModuleExtensions
    {
        public static void AddSystemServerModule(this IServiceCollection services)
        {
            services
                .AddSingleton<ServerModuleRepository, ServerModuleInMemoryRepository>();
        }
        public static void UseSystemServerModule(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(new LoadServerModuleSystemEvent())
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}