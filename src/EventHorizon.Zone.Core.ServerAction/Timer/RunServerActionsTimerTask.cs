using EventHorizon.TimerService;
using EventHorizon.Zone.Core.ServerAction.Run;
using MediatR;

namespace EventHorizon.Zone.Core.ServerAction.Timer
{
    public class RunServerActionsTimerTask : ITimerTask
    {
        public int Period { get; } = 10;
        public string Tag { get; } = "RunServerActions";
        public INotification OnRunEvent { get; } = new RunPendingServerActionsEvent();
    }
}