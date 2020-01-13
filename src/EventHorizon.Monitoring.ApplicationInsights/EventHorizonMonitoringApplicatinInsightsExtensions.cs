using System;
using EventHorizon.Monitoring.ApplicationInsights.Telemetry;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Monitoring
{
    public static class EventHorizonMonitoringApplicatinInsightsExtensions
    {
        public static IServiceCollection AddEventHorizonMonitoringApplicationInsights(
            this IServiceCollection services,
            Action<ApplicationInsightsServiceOptions> options
        ) => services
            .AddApplicationInsightsTelemetry(
                options
            )
            .AddSingleton<ITelemetryInitializer, NodeNameFilter>()
        ;

        public static IApplicationBuilder UseEventHorizonMonitoringApplicatinInsights(
            this IApplicationBuilder app
        ) => app
        ;
    }
}