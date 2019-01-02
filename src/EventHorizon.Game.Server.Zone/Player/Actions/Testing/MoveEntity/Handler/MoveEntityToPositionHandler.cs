using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
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
            await _mediator.Publish(new StartAgentMoveRoutineEvent
            {
                EntityId = notification.EntityId,
                ToPosition = notification.Position
            });
        }
    }
}