using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
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
        ;

        public static IApplicationBuilder UseEventHorizonMonitoringApplicatinInsights(
            this IApplicationBuilder app
        ) => app
        ;
    }
}