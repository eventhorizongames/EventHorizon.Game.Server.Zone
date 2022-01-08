namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Load;
using EventHorizon.Zone.System.Gui.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemGuiExtensions
{
    public static IServiceCollection AddSystemGui(
        this IServiceCollection services
    ) => services
        .AddSingleton<GuiState, InMemoryGuiState>()
    ;

    public static IApplicationBuilder UseSystemGui(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand(
        new LoadSystemGuiCommand()
    );
}
