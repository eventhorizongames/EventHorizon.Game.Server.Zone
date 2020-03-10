using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.Path;
using EventHorizon.Zone.System.Agent.Model.State;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Move.Queue
{
    // TODO: Make this a Request Command instead of a Notification Event
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
            var pathState = agent.GetProperty<PathState>(
                PathState.PROPERTY_NAME
            );
            pathState.Path = notification.Path;
            pathState.MoveTo = notification.MoveTo;
            agent.SetProperty(
                PathState.PROPERTY_NAME,
                pathState
            );
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