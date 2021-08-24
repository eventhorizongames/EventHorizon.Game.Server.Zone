namespace EventHorizon.Server.Core.Timer
{
    using EventHorizon.Server.Core.Events.Ping;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;

    using MediatR;

    public class PingCoreServerTimerTask
        : ITimerTask
    {
        public int Period { get; } = 5000; // Every 5 Seconds
        public string Tag { get; } = "PingCoreServer";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new PingCoreServer();
        public bool LogDetails { get; }
    }
}
