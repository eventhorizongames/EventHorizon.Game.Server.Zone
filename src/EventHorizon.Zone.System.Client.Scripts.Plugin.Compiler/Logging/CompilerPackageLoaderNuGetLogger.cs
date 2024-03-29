﻿namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Logging;

using global::System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NuGet.Common;

using LogLevel = NuGet.Common.LogLevel;

public class CompilerPackageLoaderNuGetLogger
    : LoggerBase
{
    private readonly ILogger<CompilerPackageLoaderNuGetLogger> _logger;

    public CompilerPackageLoaderNuGetLogger(
        IServiceCollection serviceCollection
    )
    {
        var provider = serviceCollection.BuildServiceProvider();
        _logger = provider.GetRequiredService<ILogger<CompilerPackageLoaderNuGetLogger>>();
    }

    public override void Log(
        ILogMessage message
    )
    {
        switch (message.Level)
        {
            case LogLevel.Debug:
                _logger.LogDebug(
                    message.ToString()
                );
                break;

            case LogLevel.Verbose:
                _logger.LogTrace(
                    message.ToString()
                );
                break;

            case LogLevel.Information:
                _logger.LogInformation(
                    message.ToString()
                );
                break;

            case LogLevel.Minimal:
                _logger.LogTrace(
                    message.ToString()
                );
                break;

            case LogLevel.Warning:
                _logger.LogWarning(
                    message.ToString()
                );
                break;

            case LogLevel.Error:
                _logger.LogError(
                    message.ToString()
                );
                break;
        }
    }

    public override Task LogAsync(
        ILogMessage message
    )
    {
        Log(
            message
        );

        return Task.CompletedTask;
    }
}
