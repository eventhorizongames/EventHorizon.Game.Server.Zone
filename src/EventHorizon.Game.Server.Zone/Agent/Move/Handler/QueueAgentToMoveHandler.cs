using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class QueueAgentToMoveHandler : INotificationHandler<QueueAgentToMoveEvent>
    {
        readonly IAgentRepository _agentRepository;
        readonly IMoveAgentRepository _moveRepository;
        public QueueAgentToMoveHandler(
            IAgentRepository agentRepository, 
            IMoveAgentRepository moveRepository
        )
        {
            _agentRepository = agentRepository;
            _moveRepository = moveRepository;
        }
        public async Task Handle(
            QueueAgentToMoveEvent notification, 
            CancellationToken cancellationToken
        )
        {
            var agent = await _agentRepository.FindById(
                notification.EntityId
            );
            agent.Path = notification.Path;
            await _agentRepository.Update(
                AgentAction.PATH, 
                agent
            );
            _moveRepository.Register(
                agent.Id
            );
        }
    }
}