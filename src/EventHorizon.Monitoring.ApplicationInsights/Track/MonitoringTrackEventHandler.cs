using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Monitoring.Events.Track;
using MediatR;
using Microsoft.ApplicationInsights;

namespace EventHorizon.Monitoring.ApplicationInsights.Track
{
    public class MonitoringTrackEventHandler : INotificationHandler<MonitoringTrackEvent>
    {
        private readonly TelemetryClient _telemetry;

        public MonitoringTrackEventHandler(
            TelemetryClient telemetry
        )
        {
            _telemetry = telemetry;
        }

        public Task Handle(
            MonitoringTrackEvent notification,
            CancellationToken cancellationToken
        )
        {
            _telemetry.TrackEvent(
                notification.Name
            );
            return Task.CompletedTask;
        }
    }
}