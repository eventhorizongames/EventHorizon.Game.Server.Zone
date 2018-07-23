using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Entity.Find;
using EventHorizon.Game.Server.Zone.Path.Find;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Actions.Testing.MoveEntity.Handler
{
    public class MoveEntityToPositionHandler : INotificationHandler<MoveEntityToPositionEvent>
    {
        readonly IMediator _mediator;
        public MoveEntityToPositionHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(MoveEntityToPositionEvent notification, CancellationToken cancellationToken)
        {
            // Get entity position
            var entity = await _mediator.Send(new GetEntityByIdEvent
            {
                EntityId = notification.EntityId,
            });
            // Get Path to node
            var path = await _mediator.Send(new FindPathEvent
            {
                From = entity.Position.CurrentPosition,
                To = notification.Position
            });
            // Register Path for Agent entity
            await _mediator.Publish(new RegisterAgentMovePathEvent
            {
                EntityId = notification.EntityId,
                Path = path
            });
        }
    }
}