using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAdminExtensions
    {
        public static IServiceCollection AddSystemAdmin(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemAdmin(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                return app;
            }
        }
    }
}