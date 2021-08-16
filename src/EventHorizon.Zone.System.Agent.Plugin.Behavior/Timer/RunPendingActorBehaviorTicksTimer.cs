namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;

    using MediatR;

    public class RunPendingActorBehaviorTicksTimer
        : ITimerTask
    {
        public int Period { get; } = 100;
        public string Tag { get; } = "RunPendingActorBehaviorTicks";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new RunPendingActorBehaviorTicks();
        public bool LogDetails { get; }
    }
}
