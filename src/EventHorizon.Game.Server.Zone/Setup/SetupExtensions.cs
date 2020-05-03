namespace EventHorizon.Game.Server.Zone.Setup
{
    using EventHorizon.Game.Server.Zone.Server.Core.Ping.Tasks;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServerSetupExtensions
    {
        public static IServiceCollection AddServerSetup(
            this IServiceCollection services
        ) => services
            .AddSingleton<ITimerTask, PingCoreServerTimerTask>()
        ;

        public static IApplicationBuilder UseServerSetup(
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