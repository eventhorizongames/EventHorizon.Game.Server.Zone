
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemBackupExtensions
    {
        public static IServiceCollection AddSystemBackup(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemBackup(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                
            }
        }
    }
}