namespace EventHorizon.BackgroundTasks;

using EventHorizon.BackgroundTasks.Api;
using EventHorizon.BackgroundTasks.HostedService;
using EventHorizon.BackgroundTasks.State;

using Microsoft.Extensions.DependencyInjection;

public static class BackgroundTasksStartupExtensions
{
    public static IServiceCollection AddBackgroundTasksServices(
        this IServiceCollection services
    ) => services
        .AddHostedService<BackgroundTasksHostedService>()
        .AddSingleton<BackgroundJobs, StandardBackgroundJobs>()
    ;
}
