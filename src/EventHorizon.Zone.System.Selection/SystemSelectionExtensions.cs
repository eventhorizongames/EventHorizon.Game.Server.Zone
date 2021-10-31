namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemSelectionExtensions
    {
        public static IServiceCollection AddSystemSelection(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemSelection(
            this IApplicationBuilder app
        ) => app;
    }
}
