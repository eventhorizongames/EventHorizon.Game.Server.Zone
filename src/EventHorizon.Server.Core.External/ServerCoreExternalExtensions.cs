
using System;
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Internal;
using EventHorizon.Server.Core.External.Model;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Server.Core
{
    public static class ServerCoreExternalExtensions
    {
        public static IServiceCollection AddServerCoreExternal(
            this IServiceCollection services,
            Action<CoreSettings> configureCoreSettings
        ) => services
            .AddSingleton<CoreServerConnectionCache, SystemCoreServerConnectionCache>()
            .AddTransient<CoreServerConnectionFactory, SystemCoreServerConnectionFactory>()
            .Configure<CoreSettings>(
                configureCoreSettings
            )
        ;
    }
}