using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Performance;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Handler
{
    public class MoveRegisteredAgentsHandler : INotificationHandler<MoveRegisteredAgentsEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        readonly IMoveAgentRepository _moveRepository;
        readonly IPerformanceTracker _performanceTracker;
        public MoveRegisteredAgentsHandler(
            ILogger<MoveRegisteredAgentsHandler> logger,
            IMediator mediator,
            IAgentRepository agentRepository,
            IMoveAgentRepository moveRepository,
            IPerformanceTracker performanceTracker)
        {
            _logger = logger;
            _mediator = mediator;
            _agentRepository = agentRepository;
            _moveRepository = moveRepository;
            _performanceTracker = performanceTracker;
        }
        public Task Handle(MoveRegisteredAgentsEvent notification, CancellationToken cancellationToken)
        {
            // PERF: Could be a problem in the future with a lot of Agents
            // Solution: Move Agent processing to Shards/Partitioned Servers/Tasks of Agents
            var entityIdList = _moveRepository.All();
            if (entityIdList.Count() > 0)
            {
                _logger.LogInformation("Agent Count: {}", entityIdList.Count());
                using (var tracker = _performanceTracker.Track("Move Registered Agents"))
                {
                    Parallel.ForEach(entityIdList, async (entityId) =>
                    {
                        var agent = await _agentRepository.FindById(entityId);
                        Queue<Vector3> path = agent.Path;
                        if (path == null)
                        {
                            await RemoveAgent(entityId);
                            return;
                        }
                        if (agent.Position.NextMoveRequest.CompareTo(DateTime.Now) >= 0)
                        {
                            return;
                        }
                        _logger.LogInformation("Agent Path Count: {}", path.Count);
                        Vector3 moveTo = agent.Position.CurrentPosition;
                        if (!path.TryDequeue(out moveTo))
                        {
                            await RemoveAgent(entityId);
                            return;
                        }
                        agent.Position = new PositionState
                        {
                            CurrentPosition = agent.Position.MoveToPosition,
                            MoveToPosition = moveTo,
                            NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND * agent.Speed),
                            CurrentZone = agent.Position.CurrentZone,
                            ZoneTag = agent.Position.ZoneTag,
                        };
                        await _agentRepository.Update(agent);
                        // Send update to Client for Entity
                        await _mediator.Publish(new ClientActionEvent
                        {
                            Action = "EntityClientMove",
                            Data = new
                            {
                                entityId = entityId,
                                moveTo
                            },
                        });
                        if (path.Count == 0)
                        {
                            await RemoveAgent(entityId);
                        }
                    });
                }
            }
            return Task.CompletedTask;
        }
        private async Task RemoveAgent(long agentId)
        {
            _moveRepository.Remove(agentId);
            await _mediator.Publish(new AgentRoutineFinishedEvent
            {
                AgentId = agentId
            });
        }
    }
}