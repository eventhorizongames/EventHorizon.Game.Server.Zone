namespace EventHorizon.BackgroundTasks.HostedService;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.BackgroundTasks.Api;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class BackgroundTasksHostedService
    : BackgroundService
{
    private readonly ILogger _logger;
    private readonly BackgroundJobs _backgroundJobs;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundTasksHostedService(
        ILogger<BackgroundTasksHostedService> logger,
        BackgroundJobs backgroundJobs,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _backgroundJobs = backgroundJobs;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken
    )
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var task = await _backgroundJobs.DequeueAsync(
                cancellationToken
            );
            try
            {
                _logger.LogInformation(
                    "Running Task: {@BackgroundTask}",
                    task
                );

                using var scope = _serviceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

                await scope.ServiceProvider
                    .GetRequiredService<ISender>()
                    .Send(
                        task,
                        cancellationToken
                    );

                _logger.LogInformation(
                    "Finished Running Task: {@BackgroundTask}",
                    task
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Run Task: {@BackgroundTask}",
                    task
                );
            }
        }
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Background Tasks Hosted Service Stopping."
        );

        await base.StopAsync(
            cancellationToken
        );
    }
}
