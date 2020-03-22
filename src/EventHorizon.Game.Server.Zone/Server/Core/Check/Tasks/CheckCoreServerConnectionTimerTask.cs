namespace EventHorizon.Game.Server.Zone.Server.Core.Check.Tasks
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;

    public class CheckCoreServerConnectionTimerTask : ITimerTask
    {
        public int Period { get; } = 1000 * 60; // Every 60 Seconds
        public string Tag { get; } = "CheckCoreServerConnection";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new CheckCoreServerConnectionEvent();
    }
}