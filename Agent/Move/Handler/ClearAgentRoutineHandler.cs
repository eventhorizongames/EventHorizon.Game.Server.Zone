using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class ClearAgentRoutineHandler : INotificationHandler<ClearAgentRoutineEvent>
    {
        readonly IMoveAgentRepository _moveAgentRepository;
        public ClearAgentRoutineHandler(IMoveAgentRepository moveAgentRepository)
        {
            _moveAgentRepository = moveAgentRepository;
        }
        public Task Handle(ClearAgentRoutineEvent notification, CancellationToken cancellationToken)
        {
            _moveAgentRepository.Remove(notification.AgentId);
            return Task.CompletedTask;
        }
    }
}