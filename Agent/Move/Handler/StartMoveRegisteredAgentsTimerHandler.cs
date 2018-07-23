using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class StartMoveRegisteredAgentsTimerHandler : INotificationHandler<StartMoveRegisteredAgentsTimerEvent>
    {
        readonly IMoveRegisteredAgentsTimer _moveTimer;
        public StartMoveRegisteredAgentsTimerHandler(IMoveRegisteredAgentsTimer moveTimer)
        {
            _moveTimer = moveTimer;
        }
        public Task Handle(StartMoveRegisteredAgentsTimerEvent notification, CancellationToken cancellationToken)
        {
            _moveTimer.Start();
            return Task.CompletedTask;
        }
    }
}