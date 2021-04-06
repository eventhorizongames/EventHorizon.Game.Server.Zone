namespace EventHorizon.Game.Timer
{
    using EventHorizon.Game.Capture;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;

    public class RunPlayerListCaptureLogicTimerTask 
        : ITimerTask
    {
        public int Period { get; } = 1000 * 1; // Every 1 second
        public string Tag { get; } = "RunPlayerListCaptureLogicTimerTask";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new RunCaptureLogicForAllPlayers();
        public bool LogDetails { get; }
    }
}
