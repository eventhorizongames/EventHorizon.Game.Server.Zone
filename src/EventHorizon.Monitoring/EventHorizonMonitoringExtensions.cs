using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Monitoring
{
    public static class EventHorizonMonitoringExtensions
    {
        public static IServiceCollection AddEventHorizonMonitoring(
            this IServiceCollection services
        ) => services
        ;

        public static IApplicationBuilder UseEventHorizonMonitoring(
            this IApplicationBuilder app
        ) => app
        ;
    }
}