using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.ServerAction.Run;
using MediatR;

namespace EventHorizon.Zone.Core.ServerAction.Timer
{
    public class RunServerActionsTimerTask : ITimerTask
    {
        public int Period { get; } = 10;
        public string Tag { get; } = "RunServerActions";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new RunPendingServerActionsEvent();
    }
}