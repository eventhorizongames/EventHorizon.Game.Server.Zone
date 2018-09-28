using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.General;
using EventHorizon.Game.Server.Zone.Agent.Get;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Core.RandomNumber;
using EventHorizon.Game.Server.Zone.Entity.Find;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Search;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Path.Find;
using EventHorizon.Game.Server.Zone.ServerAction.Add;
using EventHorizon.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Flee.Handler
{
    public class StartAgentFleeRoutineHandler : INotificationHandler<StartAgentFleeRoutineEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IRandomNumberGenerator _random;

        public StartAgentFleeRoutineHandler(ILogger<StartAgentFleeRoutineHandler> logger,
            IMediator mediator,
            IRandomNumberGenerator random)
        {
            _logger = logger;
            _mediator = mediator;
            _random = random;
        }
        public async Task Handle(StartAgentFleeRoutineEvent notification, CancellationToken cancellationToken)
        {
            var agent = await _mediator.Send(new GetAgentEvent
            {
                AgentId = notification.AgentId
            });
            if (!agent.IsFound())
            {
                return;
            }
            if (AiRoutine.FLEEING.Equals(agent.GetProperty<AiRoutine>("Routine")) && agent.Path?.Count > 2)
            {
                // Agent is Fleeing, check again in the future.
                await this.CheckFleeInFuture(agent);
                return;
            }
            // Find entity in sight
            var entitiesInSight = (await _mediator.Send(new SearchInAreaWithTagEvent
            {
                SearchPositionCenter = agent.Position.CurrentPosition,
                SearchRadius = agent.GetProperty<AgentAiState>("Ai").Flee.SightDistance,
                TagList = agent.GetProperty<AgentAiState>("Ai").Flee.TagList,
            })).Where(a => a != agent.Id);
            if (entitiesInSight.IsEmpty())
            {
                if (agent.GetProperty<AiRoutine>("Routine") == AiRoutine.FLEE)
                {
                    await _mediator.Publish(new StartAgentRoutineEvent
                    {
                        AgentId = agent.Id,
                        Routine = agent.GetProperty<AgentAiState>("Ai").Flee.FallbackRoutine
                    });
                }
                // Set Agents Routine
                await this.CheckFleeInFuture(agent);
                return;
            }

            // TODO: In future make it so each entity is combined and Agent is fleeing form position between them all.
            var entityInSight = await _mediator.Send(new GetEntityByIdEvent
            {
                EntityId = entitiesInSight.First()
            });
            if (!entityInSight.IsFound())
            {
                if (agent.GetProperty<AiRoutine>("Routine") == AiRoutine.FLEE)
                {
                    await _mediator.Publish(new StartAgentRoutineEvent
                    {
                        AgentId = agent.Id,
                        Routine = agent.GetProperty<AgentAiState>("Ai").Flee.FallbackRoutine
                    });
                }
                // Set Agents Routine
                await this.CheckFleeInFuture(agent);
                return;
            }
            var away = this.GetPositionAwayFromAgentAndEntityInSight(agent, entityInSight, agent.GetProperty<AgentAiState>("Ai").Flee.DistanceToRun);
            // Get Path away from entity
            var path = await _mediator.Send(new FindPathEvent
            {
                From = agent.Position.CurrentPosition,
                To = away
            });
            _logger.LogInformation("Found Entity: {0}, Position: {1}", entityInSight.Id, entityInSight.Position.CurrentPosition);
            if (path.IsEmpty())
            {
                if (agent.GetProperty<AiRoutine>("Routine") == AiRoutine.FLEE)
                {
                    await _mediator.Publish(new StartAgentRoutineEvent
                    {
                        AgentId = agent.Id,
                        Routine = agent.GetProperty<AgentAiState>("Ai").Flee.FallbackRoutine
                    });
                }
                // Set Agents Routine
                await this.CheckFleeInFuture(agent);
                return;
            }
            // Set Agents Routine
            await _mediator.Publish(new SetAgentRoutineEvent
            {
                AgentId = agent.Id,
                Routine = AiRoutine.FLEEING
            });
            // Register Path for Agent entity
            await _mediator.Publish(new RegisterAgentMovePathEvent
            {
                AgentId = agent.Id,
                Path = path
            });

            await this.CheckFleeInFuture(agent);
        }

        private Vector3 GetPositionAwayFromAgentAndEntityInSight(AgentEntity agent, IObjectEntity entityInSight, float distanceToRun)
        {
            return Vector3.Add(agent.Position.CurrentPosition, Vector3.Multiply(distanceToRun, NormalizeDirection(
                Vector3.Subtract(agent.Position.CurrentPosition, entityInSight.Position.CurrentPosition)
            )));
        }
        private Vector3 NormalizeDirection(Vector3 direction)
        {
            var normal = Vector3.Normalize(direction);
            if (InvalidVector3(normal))
            {
                var randomDistance = _random.Next(1, 4);
                switch (randomDistance)
                {
                    case 1:
                        return Vector3.UnitX;
                    case 2:
                        return Vector3.UnitZ;
                    case 3:
                        return new Vector3(-1, 0, 0);
                    default:
                        return new Vector3(0, 0, -1);
                }
            }
            return normal;
        }

        private bool InvalidVector3(Vector3 vector)
        {
            return float.IsNaN(vector.X) || float.IsNaN(vector.Z) || float.IsNaN(vector.Y);
        }

        private async Task CheckFleeInFuture(AgentEntity agent)
        {
            await _mediator.Publish(new AddServerActionEvent(DateTime.UtcNow.AddSeconds(1), new StartAgentFleeRoutineEvent
            {
                AgentId = agent.Id
            }));
        }
    }
}