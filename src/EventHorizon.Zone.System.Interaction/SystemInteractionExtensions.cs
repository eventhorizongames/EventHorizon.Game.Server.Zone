
using EventHorizon.Zone.System.Interaction.Script.Load;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemInteractionExtensions
    {
        public static IServiceCollection AddSystemInteraction(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemInteraction(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Send(
                    new LoadInteractionScriptsCommand()
                ).GetAwaiter().GetResult();
            }
        }
    }
}