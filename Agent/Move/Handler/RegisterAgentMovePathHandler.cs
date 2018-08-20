using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class RegisterAgentMovePathHandler : INotificationHandler<RegisterAgentMovePathEvent>
    {
        readonly IAgentRepository _agentRepository;
        readonly IMoveAgentRepository _moveRepository;
        public RegisterAgentMovePathHandler(IAgentRepository agentRepository, IMoveAgentRepository moveRepository)
        {
            _agentRepository = agentRepository;
            _moveRepository = moveRepository;
        }
        public async Task Handle(RegisterAgentMovePathEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.FindById(notification.EntityId);
            agent.Path = notification.Path;
            await _agentRepository.Update(agent);
            _moveRepository.Add(agent.Id);
        }
    }
}