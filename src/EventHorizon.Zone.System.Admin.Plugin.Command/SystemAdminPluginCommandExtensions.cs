namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
    using EventHorizon.Zone.System.Admin.Plugin.Command.State;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAdminPluginCommandExtensions
    {
        public static IServiceCollection AddSystemAdminPluginCommand(
            this IServiceCollection services
        ) => services
            .AddSingleton<AdminCommandRepository, AdminCommandInMemoryRepository>()
        ;

        public static IApplicationBuilder UseSystemAdminPluginCommand(
            this IApplicationBuilder app
        ) => app.SendMediatorCommand(
            new LoadAdminCommands()
        );
    }
}
