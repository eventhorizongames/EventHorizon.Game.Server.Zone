
using EventHorizon.Zone.Plugin.Interaction.Script.Load;
using EventHorizon.Zone.Plugin.Interaction.Script.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginInteractionExtensions
    {
        public static IServiceCollection AddPluginInteraction(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<InteractionScriptRepository, InMemoryInteractionScriptRepository>()
            ;
        }
        public static void UsePluginInteraction(
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