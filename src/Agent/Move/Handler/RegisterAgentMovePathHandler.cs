using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class RegisterAgentMovePathHandler : INotificationHandler<RegisterAgentMovePathEvent>
    {
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        readonly IMoveAgentRepository _moveRepository;
        public RegisterAgentMovePathHandler(IMediator mediator, IAgentRepository agentRepository, IMoveAgentRepository moveRepository)
        {
            _mediator = mediator;
            _agentRepository = agentRepository;
            _moveRepository = moveRepository;
        }
        public async Task Handle(RegisterAgentMovePathEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.FindById(notification.AgentId);
            agent.Path = notification.Path;
            await _agentRepository.Update(AgentAction.PATH, agent);
            _moveRepository.Add(agent.Id);
        }
    }
}