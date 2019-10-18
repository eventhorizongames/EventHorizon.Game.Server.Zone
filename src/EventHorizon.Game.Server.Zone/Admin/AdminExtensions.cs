using EventHorizon.Game.Server.Zone.Admin.FileSystem;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class AdminExtensions
    {
        public static IServiceCollection AddServerAdmin(
            this IServiceCollection services
        ) => services;
        
        public static IApplicationBuilder UseServerAdmin(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(
                        new StartAdminFileSystemWatchingCommand()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}