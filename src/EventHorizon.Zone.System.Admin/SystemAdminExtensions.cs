namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAdminExtensions
    {
        public static IServiceCollection AddSystemAdmin(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemAdmin(
            this IApplicationBuilder app
        ) => app;
    }
}
