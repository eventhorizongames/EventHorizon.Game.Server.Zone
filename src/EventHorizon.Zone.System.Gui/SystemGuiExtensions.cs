
using EventHorizon.Zone.System.Gui.Load;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemGuiExtensions
    {
        public static IServiceCollection AddSystemGui(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemGui(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadSystemGuiCommand())
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}