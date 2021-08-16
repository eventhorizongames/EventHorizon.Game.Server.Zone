namespace EventHorizon.Performance
{
    using EventHorizon.Performance.Model;

    using Microsoft.Extensions.DependencyInjection;

    public static class PerformanceExtensions
    {
        public static IServiceCollection AddPerformance(
            this IServiceCollection services
        ) => services
            .AddSingleton<PerformanceSettings, PerformanceSettingsByConfiguration>()
            .AddSingleton<PerformanceTrackerFactory, ToLoggerPerformanceTrackerFactory>()
        ;
    }
}
