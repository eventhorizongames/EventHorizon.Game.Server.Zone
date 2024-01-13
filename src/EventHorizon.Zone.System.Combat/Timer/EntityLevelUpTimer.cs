namespace EventHorizon.Zone.System.Combat.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Combat.Events.Level;

using MediatR;

public class EntityLevelUpTimer
    : ITimerTask
{
    public int Period { get; } = 100;
    public string Tag { get; } = "EntityLevelUp";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new EntityLevelUpFromQueueEvent();
    public bool LogDetails { get; }
}
