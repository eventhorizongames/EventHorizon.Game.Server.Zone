using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.Core.Reporter.Timer;
using EventHorizon.Zone.Core.Reporter.Tracker;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class CoreReporterExtensions
    {
        public static IServiceCollection AddCoreReporter(
            this IServiceCollection services
        )
        {
            var repo = new ToRepositoryReportTracker();
            return services
                .AddSingleton<ReportTracker>(repo)
                .AddSingleton<ReportRepository>(repo)
                .AddSingleton<ITimerTask, SavePendingReportItemsTimer>()
            ;
        }
    }
}