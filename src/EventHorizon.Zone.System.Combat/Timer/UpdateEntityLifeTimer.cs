namespace EventHorizon.Zone.System.Combat.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Combat.Events.Life;

using MediatR;

public class UpdateEntityLifeTimer
    : ITimerTask
{
    public int Period { get; } = 100;
    public string Tag { get; } = "UpdateEntityLife";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new UpdateEntityLifeFromQueueEvent();
    public bool LogDetails { get; }
}
