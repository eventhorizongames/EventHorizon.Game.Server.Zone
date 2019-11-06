
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Connection.Internal;
using EventHorizon.Zone.System.Player.Connection.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemPlayerConnectionExtensions
    {
        public static IServiceCollection AddSystemPlayerConnection(
            this IServiceCollection services,
            // TODO: Update to use Action<PlayerServerConnectionSettings> options
            IConfiguration configuration
        )
        {
            return services
                .AddSingleton<PlayerServerConnectionCache, SystemPlayerServerConnectionCache>()
                .AddTransient<PlayerServerConnectionFactory, SystemPlayerServerConnectionFactory>()
                .Configure<PlayerServerConnectionSettings>(
                    configuration.GetSection("Player")
                )
            ;
        }
    }
}