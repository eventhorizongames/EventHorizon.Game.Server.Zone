namespace EventHorizon.Monitoring.ApplicationInsights.Track;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Monitoring.Events.Track;

using MediatR;

using Microsoft.ApplicationInsights;

public class MonitoringTrackMetricEventHandler : INotificationHandler<MonitoringTrackMetricEvent>
{
    private readonly TelemetryClient _telemetry;

    public MonitoringTrackMetricEventHandler(
        TelemetryClient telemetry
    )
    {
        _telemetry = telemetry;
    }

    public Task Handle(
        MonitoringTrackMetricEvent notification,
        CancellationToken cancellationToken
    )
    {
        _telemetry.TrackMetric(
            notification.Name,
            notification.Average
        );
        return Task.CompletedTask;
    }
}
