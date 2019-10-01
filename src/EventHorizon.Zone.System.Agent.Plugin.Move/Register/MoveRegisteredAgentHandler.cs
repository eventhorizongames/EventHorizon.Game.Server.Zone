using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Performance;
using MediatR;
using Microsoft.Extensions.Logging;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Model.State;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

namespace EventHorizon.Zone.System.Agent.Move.Register
{
    public class MoveRegisteredAgentHandler : INotificationHandler<MoveRegisteredAgentEvent>
    {
        public const int AGENT_MOVE_MULTIPLIER = 10;
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IDateTimeService _dateTime;
        readonly IAgentRepository _agentRepository;
        readonly IPerformanceTracker _performanceTracker;
        public MoveRegisteredAgentHandler(
            ILogger<MoveRegisteredAgentHandler> logger,
            IMediator mediator,
            IDateTimeService dateTime,
            IAgentRepository agentRepository,
            IPerformanceTracker performanceTracker
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateTime = dateTime;
            _agentRepository = agentRepository;
            _performanceTracker = performanceTracker;
        }

        public async Task Handle(
            MoveRegisteredAgentEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entityId = notification.EntityId;
            var agent = await _agentRepository.FindById(
                entityId
            );
            var path = agent.Path;
            if (path == null)
            {
                await RemoveAgent(
                    agent
                );
                return;
            }
            // Agent time to move has not expired or can is not set to Move, ignore request
            if (
                agent.Position.NextMoveRequest.CompareTo(
                    _dateTime.Now
                ) >= 0 || !agent.Position.CanMove
            )
            {
                await QueueForNextUpdateCycle(
                    entityId,
                    path
                );
                return;
            }
            if (path.Count <= 0)
            {
                await RemoveAgent(
                    agent
                );
                return;
            }
            var moveTo = path.Dequeue();

            // TODO: Create Position update logic service
            var newPosition = agent.Position;
            newPosition.CurrentPosition = agent.Position.MoveToPosition;
            newPosition.MoveToPosition = moveTo;
            newPosition.NextMoveRequest = _dateTime.Now.AddMilliseconds(
                AGENT_MOVE_MULTIPLIER
            );
            agent.Position = newPosition;
            await _agentRepository.Update(
                EntityAction.POSITION,
                agent
            );
            using (_performanceTracker.Track(
                "ClientActionEntityClientMoveToAllEvent"
            ))
            {
                // Send update to Client for Entity
                await _mediator.Publish(
                    new ClientActionEntityClientMoveToAllEvent
                    {
                        Data = new EntityClientMoveData
                        {
                            EntityId = entityId,
                            MoveTo = moveTo
                        },
                    }
                );
            }
            if (path.Count == 0)
            {
                await RemoveAgent(
                    agent
                );
                return;
            }
            await QueueForNextUpdateCycle(
                entityId,
                path
            );
        }
        private async Task RemoveAgent(
            AgentEntity entity
        )
        {
            using (_performanceTracker.Track(
                "AgentFinishedMoveEvent"
            ))
            {
                entity.Path = null;
                await _mediator.Publish(
                    new AgentFinishedMoveEvent
                    {
                        EntityId = entity.Id,
                    }
                );
            }
        }
        private async Task QueueForNextUpdateCycle(
            long entityId,
            Queue<Vector3> path
        )
        {
            using (_performanceTracker.Track(
                "QueueForNextUpdateCycle"
            ))
            {
                await _mediator.Publish(
                    new QueueAgentToMoveEvent
                    {
                        EntityId = entityId,
                        Path = path
                    }
                );
            }
        }
    }
}