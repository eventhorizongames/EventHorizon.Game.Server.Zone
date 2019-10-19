using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAdminPluginCommandExtensions
    {
        public static IServiceCollection AddSystemAdminPluginCommand(
            this IServiceCollection services
        ) => services
            .AddSingleton<AdminCommandRepository, AdminCommandInMemoryRepository>()
        ;

        public static IApplicationBuilder UseSystemAdminPluginCommand(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(
                        new LoadAdminCommands()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}