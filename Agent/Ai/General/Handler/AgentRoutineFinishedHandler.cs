using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler
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
            var agent = await _agentRepository.FindById(notification.AgentId);
            if (agent.IsFound())
            {
                await _mediator.Publish(new ClearAgentRoutineEvent { AgentId = agent.Id });
                await _mediator.Publish(new RunAgentDefaultRoutineEvent
                {
                    AgentId = agent.Id
                });
            }
        }
    }
}