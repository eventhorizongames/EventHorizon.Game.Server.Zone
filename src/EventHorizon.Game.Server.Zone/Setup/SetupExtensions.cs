namespace EventHorizon.Game.Server.Zone.Setup;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class ServerSetupExtensions
{
    public static IServiceCollection AddServerSetup(
        this IServiceCollection services
    ) => services
    ;

    public static IApplicationBuilder UseServerSetup(
        this IApplicationBuilder app
    ) => app;
}
