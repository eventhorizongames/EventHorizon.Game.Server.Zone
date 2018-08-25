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
    public class MoveRegisteredAgentHandler : INotificationHandler<MoveRegisteredAgentEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IAgentRepository _agentRepository;
        readonly IMoveAgentRepository _moveRepository;
        readonly IPerformanceTracker _performanceTracker;
        public MoveRegisteredAgentHandler(
            ILogger<MoveRegisteredAgentHandler> logger,
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
        public async Task Handle(MoveRegisteredAgentEvent notification, CancellationToken cancellationToken)
        {
            var agentId = notification.AgentId;
            var agent = await _agentRepository.FindById(agentId);
            Queue<Vector3> path = agent.Path;
            if (path == null)
            {
                await RemoveAgent(agentId);
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
                await RemoveAgent(agentId);
                return;
            }
            agent.Position = new PositionState
            {
                CurrentPosition = agent.Position.MoveToPosition,
                MoveToPosition = moveTo,
                NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND / agent.Speed),
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
                    entityId = agentId,
                    moveTo
                },
            });
            if (path.Count == 0)
            {
                await RemoveAgent(agentId);
            }
        }
        private async Task RemoveAgent(long agentId)
        {
            _moveRepository.Remove(agentId);
            await _mediator.Publish(new AgentFinishedMoveEvent
            {
                AgentId = agentId,
            });
        }
    }
}