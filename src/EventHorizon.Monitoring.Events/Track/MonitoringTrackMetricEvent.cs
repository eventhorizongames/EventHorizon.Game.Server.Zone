namespace EventHorizon.Monitoring.Events.Track;

using MediatR;

public struct MonitoringTrackMetricEvent
    : INotification
{
    public string Name { get; }
    public double Average { get; }

    public MonitoringTrackMetricEvent(
        string name,
        double average
    )
    {
        Name = name;
        Average = average;
    }
}
