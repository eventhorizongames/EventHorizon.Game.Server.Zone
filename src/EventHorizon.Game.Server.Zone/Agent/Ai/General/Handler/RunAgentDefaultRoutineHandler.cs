using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General.Handler
{
    public class RunAgentDefaultRoutineHandler : INotificationHandler<RunAgentDefaultRoutineEvent>
    {
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        public RunAgentDefaultRoutineHandler(IMediator mediator, IAgentRepository agentRepository)
        {
            _mediator = mediator;
            _agentRepository = agentRepository;
        }
        public async Task Handle(RunAgentDefaultRoutineEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.FindById(notification.AgentId);
            if (agent.IsFound())
            {
                agent.SetProperty("Routine", agent.GetProperty<AgentAiState>("Ai").DefaultRoutine);
                await _mediator.Publish(new StartAgentRoutineEvent
                {
                    AgentId = agent.Id,
                    Routine = agent.GetProperty<AiRoutine>("Routine")
                });
            }
        }
    }
}