using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
{
    public class AgentRoutineFinishedHandler : INotificationHandler<AgentRoutineFinishedEvent>
    {
        readonly IAgentRepository _agentRepository;
        readonly IMediator _mediator;
        public AgentRoutineFinishedHandler(IMediator mediator, IAgentRepository agentRepository)
        {
            _mediator = mediator;
            _agentRepository = agentRepository;
        }
        public async Task Handle(AgentRoutineFinishedEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.FindById(notification.EntityId);
            if (agent.IsFound())
            {
                await _mediator.Publish(new ClearAgentRoutineEvent { EntityId = agent.Id });
                await _mediator.Publish(new RunAgentDefaultRoutineEvent
                {
                    EntityId = agent.Id
                });
            }
        }
    }
}