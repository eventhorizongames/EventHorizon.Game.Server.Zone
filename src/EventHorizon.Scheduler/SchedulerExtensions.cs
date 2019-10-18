using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventHorizon.Schedule
{
    public static class SchedulerExtensions
    {
        public static IServiceCollection AddScheduler(
            this IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler
        ) => services
            .AddSingleton<IHostedService, SchedulerHostedService>(
                serviceProvider =>
                {
                    var instance = new SchedulerHostedService(
                        serviceProvider.GetServices<IScheduledTask>()
                    );
                    instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                    return instance;
                }
            )
        ;
    }
}