using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Move.Timer
{
    public class MoveRegisteredAgentsTimer : ITimerTask
    {
        public int Period { get; } = 100;
        public string Tag { get; } = "MoveRegisteredAgents";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new MoveRegisteredAgentsEvent();
    }
}