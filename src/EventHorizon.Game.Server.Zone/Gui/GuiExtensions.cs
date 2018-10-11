using System.Reflection;
using EventHorizon.Game.Server.Zone.Gui.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Gui
{
    public static class GuiExtensions
    {
        public static void AddGui(this IServiceCollection services)
        {
            services
                .AddSingleton<GuiState, GuiStateContainer>();
        }
        public static void UseGui(this IApplicationBuilder app)
        {
            // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            // {
            // }
        }
    }
}