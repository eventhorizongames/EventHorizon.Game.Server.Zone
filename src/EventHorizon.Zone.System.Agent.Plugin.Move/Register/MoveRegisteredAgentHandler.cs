namespace EventHorizon.Zone.System.Agent.Move.Register
{
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Move;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.Path;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class MoveRegisteredAgentHandler : INotificationHandler<MoveRegisteredAgentEvent>
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTime;
        private readonly IAgentRepository _agentRepository;

        public MoveRegisteredAgentHandler(
            IMediator mediator,
            IDateTimeService dateTime,
            IAgentRepository agentRepository
        )
        {
            _mediator = mediator;
            _dateTime = dateTime;
            _agentRepository = agentRepository;
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
            if (!agent.IsFound())
            {
                return;
            }
            var pathState = agent.GetProperty<PathState>(
                PathState.PROPERTY_NAME
            );
            var path = pathState.Path();
            if (path == null)
            {
                await RemoveAgent(
                    agent,
                    pathState
                );
                return;
            }
            // Agent time to move has not expired or can is not set to Move, ignore request
            var locationState = agent.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            if (
                locationState.NextMoveRequest.CompareTo(
                    _dateTime.Now
                ) >= 0 || !locationState.CanMove
            )
            {
                await QueueForNextUpdateCycle(
                    entityId,
                    pathState.MoveTo,
                    path
                );
                return;
            }
            if (path.Count <= 0)
            {
                await RemoveAgent(
                    agent,
                    pathState
                );
                return;
            }

            await _mediator.Send(
                new MoveEntityToPositionCommand(
                    agent,
                    path.Dequeue(),
                    false
                )
            );

            if (path.Count == 0)
            {
                await RemoveAgent(
                    agent,
                    pathState
                );
                return;
            }
            await QueueForNextUpdateCycle(
                entityId,
                pathState.MoveTo,
                path
            );
        }
        private async Task RemoveAgent(
            AgentEntity entity,
            PathState pathState
        )
        {
            pathState.SetPath(null);
            entity.SetProperty(
                PathState.PROPERTY_NAME,
                pathState
            );
            await _mediator.Send(
                new UpdateEntityCommand(
                    AgentAction.PATH,
                    entity
                )
            );
            await _mediator.Publish(
                new AgentFinishedMoveEvent
                {
                    EntityId = entity.Id,
                }
            );
        }
        private async Task QueueForNextUpdateCycle(
            long entityId,
            Vector3 moveTo,
            Queue<Vector3> path
        )
        {
            await _mediator.Send(
                new QueueAgentToMove(
                    entityId,
                    path,
                    moveTo
                )
            );
        }
    }
}