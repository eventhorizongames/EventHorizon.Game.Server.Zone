namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemBackupExtensions
    {
        public static IServiceCollection AddSystemBackup(
            this IServiceCollection services
        )
        {
            return services;
        }

        public static IApplicationBuilder UseSystemBackup(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}
