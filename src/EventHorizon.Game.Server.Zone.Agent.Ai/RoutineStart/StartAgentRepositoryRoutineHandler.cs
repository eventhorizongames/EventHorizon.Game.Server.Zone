using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.State;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.RoutineStart
{
    public struct StartAgentRepositoryRoutineHandler : INotificationHandler<StartAgentRoutineEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IScriptServices _scriptServices;
        readonly IAgentRoutineRepository _routineRepository;

        public StartAgentRepositoryRoutineHandler(
            ILogger<StartAgentRepositoryRoutineHandler> logger,
            IMediator mediator,
            IScriptServices scriptServices,
            IAgentRoutineRepository routineRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _scriptServices = scriptServices;
            _routineRepository = routineRepository;
        }
        public async Task Handle(StartAgentRoutineEvent notification, CancellationToken cancellationToken)
        {
            // Get the Agent Entity
            var agent = await _mediator.Send(new GetAgentEvent
            {
                EntityId = notification.EntityId,
            });
            var routineScript = _routineRepository.Find(
                notification.Routine.Name
            );
            if (!agent.IsFound() || !routineScript.IsFound())
            {
                _logger.LogWarning("Routine ({Routine}) or Agent ({AgentId}) not found.", notification.Routine.Name, notification.EntityId);
                return;
            }
            // Clear any already in process Routines
            await _mediator.Publish(new ClearAgentRoutineEvent
            {
                EntityId = agent.Id
            });
            // Set Agents Routine
            await _mediator.Publish(new SetAgentRoutineEvent
            {
                EntityId = agent.Id,
                Routine = new AgentRoutine(routineScript.RoutineName)
            });
            await routineScript.Run(_scriptServices, agent, notification.Data).ConfigureAwait(false);
        }
    }
}