namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Monitor.Path;
    using MediatR;

    public class CheckForStaleAgentPathTimer
        : ITimerTask
    {
        public int Period { get; } = 1000;
        public string Tag { get; } = "CheckForStaleAgentPath";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new CheckForStaleAgentPath();
        public bool LogDetails { get; }
    }
}