namespace EventHorizon.Monitoring
{
    using System;
    using EventHorizon.Monitoring.Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class EventHorizonMonitoringExtensions
    {
        public static IServiceCollection AddEventHorizonMonitoring(
            this IServiceCollection services,
            Action<MonitoringServerConfiguration> options
        ) => services
            .Configure<MonitoringServerConfiguration>(
                options
            )
        ;

        public static IApplicationBuilder UseEventHorizonMonitoring(
            this IApplicationBuilder app
        ) => app
        ;
    }
}