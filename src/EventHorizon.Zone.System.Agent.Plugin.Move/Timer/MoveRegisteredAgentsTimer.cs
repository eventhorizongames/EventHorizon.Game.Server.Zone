namespace EventHorizon.Zone.System.Agent.Move.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

using MediatR;

public class MoveRegisteredAgentsTimer
    : ITimerTask
{
    public int Period { get; } = 50;
    public string Tag { get; } = "MoveRegisteredAgents";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new MoveRegisteredAgentsEvent();
    public bool LogDetails { get; }
}
