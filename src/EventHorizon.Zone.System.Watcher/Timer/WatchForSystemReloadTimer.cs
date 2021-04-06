namespace EventHorizon.Zone.System.Watcher.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Watcher.Check;
    using MediatR;

    public class WatchForSystemReloadTimer 
        : ITimerTask
    {
        public int Period { get; } = 5000;
        public string Tag { get; } = "WatchForSystemReload";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new CheckPendingReloadEvent();
        public bool LogDetails { get; }
    }
}