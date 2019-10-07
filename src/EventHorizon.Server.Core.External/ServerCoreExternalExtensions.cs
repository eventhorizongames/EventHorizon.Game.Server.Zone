
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Server.Core
{
    public static class ServerCoreExternalExtensions
    {
        public static IServiceCollection AddServerCoreExternal(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<CoreServerConnectionCache, SystemCoreServerConnectionCache>()
                .AddTransient<CoreServerConnectionFactory, SystemCoreServerConnectionFactory>()
            ;
        }
    }
}