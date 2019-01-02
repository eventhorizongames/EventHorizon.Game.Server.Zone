using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Events;
using EventHorizon.Game.Server.Zone.Agent.Move;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Handlers
{
    public class AgentFinishedMoveHandler : INotificationHandler<AgentFinishedMoveEvent>
    {
        readonly IMediator _mediator;
        public AgentFinishedMoveHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(AgentFinishedMoveEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new AgentRoutineFinishedEvent
            {
                EntityId = notification.EntityId
            });
        }
    }
}