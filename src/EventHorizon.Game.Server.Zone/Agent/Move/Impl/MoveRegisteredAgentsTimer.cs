using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Events.Move;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimer : ITimerTask
    {
        public int Period { get; } = 100;
        public string Tag { get; } = "MoveRegisteredAgents";
        public INotification OnRunEvent { get; } = new MoveRegisteredAgentsEvent();
    }
}