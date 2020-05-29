namespace EventHorizon.Game.Timer
{
    using EventHorizon.Game.Capture;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using MediatR;

    public class RunPlayerListCaptureLogicTimerTask : ITimerTask
    {
        public int Period => 1000 * 1; // Every 1 second
        public string Tag => "RunPlayerListCaptureLogicTimerTask";
        public IRequest<bool> OnValidationEvent => new IsServerStarted();
        public INotification OnRunEvent => new RunCaptureLogicForAllPlayers();
    }
}
