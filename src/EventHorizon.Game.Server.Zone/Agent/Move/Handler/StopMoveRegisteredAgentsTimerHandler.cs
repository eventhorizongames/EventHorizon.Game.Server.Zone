using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class StopMoveRegisteredAgentsTimerHandler : INotificationHandler<StopMoveRegisteredAgentsTimerEvent>
    {
        readonly IMoveRegisteredAgentsTimer _moveTimer;
        public StopMoveRegisteredAgentsTimerHandler(IMoveRegisteredAgentsTimer moveTimer)
        {
            _moveTimer = moveTimer;
        }
        public Task Handle(StopMoveRegisteredAgentsTimerEvent notification, CancellationToken cancellationToken)
        {
            _moveTimer.Stop();
            return Task.CompletedTask;
        }
    }
}