namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Settings;
    using EventHorizon.Zone.Core.Reporter.Timer;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using EventHorizon.Zone.Core.Reporter.Writer.Client;
    using EventHorizon.Zone.Core.Reporter.Writer.Client.Startup;
    using EventHorizon.Zone.Core.Reporter.Writer.Client.Timer;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreReporterExtensions
    {
        public static IServiceCollection AddCoreReporter(
            this IServiceCollection services
        ) => services
            .AddSingleton<ITimerTask, CheckElasticsearchReporterClientConnectionTimerTask>()
            .AddSingleton<ElasticsearchReporterClientBasedOnElasticClient>()
            .AddSingleton<ElasticsearchReporterClient>(
                services => services.GetRequiredService<ElasticsearchReporterClientBasedOnElasticClient>()
            ).AddSingleton<ElasticsearchReporterClientStartup>(
                services => services.GetRequiredService<ElasticsearchReporterClientBasedOnElasticClient>()
            ).AddSingleton<ReportTrackingRepository>()
            .AddSingleton<ReportTrackingRepositoryBySettings>()
            .AddSingleton<ReporterSettings, ReporterSettingsByConfiguration>()
            .AddSingleton<ReportTracker>(
                services => services.GetRequiredService<ReportTrackingRepositoryBySettings>()
            ).AddSingleton<ReportRepository>(
                services => services.GetRequiredService<ReportTrackingRepositoryBySettings>()
            ).AddSingleton<ITimerTask, SavePendingReportItemsTimer>()
        ;

        public static IApplicationBuilder UseCoreReporter(
            this IApplicationBuilder app
        )
        {
            app.SendMediatorCommand(
                new StartupElasticsearchReporterClient()
            );
            
            return app;
        }
    }
}
