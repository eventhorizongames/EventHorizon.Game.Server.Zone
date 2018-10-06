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
            var agent = await _agentRepository.FindById(notification.AgentId);
            if (agent.IsFound())
            {
                agent.SetProperty("Routine", agent.GetProperty<AgentRoutine>("DefaultRoutine"));
                var currentRoutine = agent.GetProperty<AgentRoutine>("Routine");
                // Clear any already in process Routines
                await _mediator.Publish(new ClearAgentRoutineEvent
                {
                    AgentId = agent.Id
                });
                await _mediator.Publish(new StartAgentRoutineEvent
                {
                    AgentId = agent.Id,
                    Routine = agent.GetProperty<AgentRoutine>("Routine")
                });
            }
        }
    }
}