namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
    using EventHorizon.Zone.System.Player.Plugin.Action.State;

    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemPlayerPluginActionExtensions
    {
        public static IServiceCollection AddSystemPlayerPluginAction(
            this IServiceCollection services
        ) => services
            .AddSingleton<PlayerActionRepository, InMemoryPlayerActionRepository>()
        ;

        public static IApplicationBuilder UseSystemPlayerPluginAction(
            this IApplicationBuilder app
        )
        {
            using var serviceScope = app.CreateServiceScope();

            serviceScope.ServiceProvider
                .GetRequiredService<IMediator>()
                .Publish(
                    new ReadyForPlayerActionRegistration()
                ).GetAwaiter().GetResult();
            return app;
        }
    }
}
