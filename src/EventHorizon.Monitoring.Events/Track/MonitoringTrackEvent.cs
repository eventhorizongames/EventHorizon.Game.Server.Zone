namespace EventHorizon.Monitoring.Events.Track
{
    using MediatR;

    public struct MonitoringTrackEvent
        : INotification
    {
        public string Name { get; }

        public MonitoringTrackEvent(
            string name
        )
        {
            Name = name;
        }
    }
}
