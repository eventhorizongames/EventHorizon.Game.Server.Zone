using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
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
            var agent = await _agentRepository.FindById(notification.EntityId);
            if (agent.IsFound())
            {
                agent.SetProperty(AgentRoutine.ROUTINE_NAME, agent.GetProperty<AgentRoutine>(AgentRoutine.DEFAULT_ROUTINE_NAME));
                var currentRoutine = agent.GetProperty<AgentRoutine>(AgentRoutine.ROUTINE_NAME);
                // Clear any already in process Routines
                await _mediator.Publish(new ClearAgentRoutineEvent
                {
                    EntityId = agent.Id
                });
                await _mediator.Publish(new StartAgentRoutineEvent
                {
                    EntityId = agent.Id,
                    Routine = agent.GetProperty<AgentRoutine>(AgentRoutine.ROUTINE_NAME)
                });
            }
        }
    }
}