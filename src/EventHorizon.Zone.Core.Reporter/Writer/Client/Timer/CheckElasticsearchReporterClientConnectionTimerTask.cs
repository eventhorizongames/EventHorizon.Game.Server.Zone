namespace EventHorizon.Zone.Core.Reporter.Writer.Client.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Reporter.Writer.Client.Check;
    using MediatR;

    public class CheckElasticsearchReporterClientConnectionTimerTask 
        : ITimerTask
    {
        public int Period { get; } = 30000; // Every 30 seconds
        public string Tag { get; } = "CheckElasticsearchReporterClientConnectionTimerTask";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new CheckElasticsearchReporterClientConnection();
        public bool LogDetails { get; } = true;
    }
}
