namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Timer;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreReporterExtensions
    {
        public static IServiceCollection AddCoreReporter(
            this IServiceCollection services
        ) => services
            .AddSingleton<ReportTrackingRepository>()
            .AddSingleton<ReportTracker>(
                services => services.GetRequiredService<ReportTrackingRepository>()
            ).AddSingleton<ReportRepository>(
                services => services.GetRequiredService<ReportTrackingRepository>()
            ).AddSingleton<ITimerTask, SavePendingReportItemsTimer>()
        ;
    }
}