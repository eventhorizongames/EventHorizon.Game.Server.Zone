namespace EventHorizon.Zone.Core.ServerAction.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.ServerAction.Run;

using MediatR;

public class RunServerActionsTimerTask
    : ITimerTask
{
    public int Period { get; } = 10;
    public string Tag { get; } = "RunServerActions";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new RunPendingServerActionsEvent();
    public bool LogDetails { get; }
}
