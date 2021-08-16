using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Player.Action.Direction;
using EventHorizon.Game.Server.Zone.Player.Move.Stop;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;

using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Move.Action
{
    public class RegisterPlayerMoveActionsHandler : INotificationHandler<ReadyForPlayerActionRegistration>
    {
        readonly IMediator _mediator;
        readonly IdPool _idPool;

        public RegisterPlayerMoveActionsHandler(
            IMediator mediator,
            IdPool idPool
        )
        {
            _mediator = mediator;
            _idPool = idPool;
        }
        public async Task Handle(
            ReadyForPlayerActionRegistration request,
            CancellationToken cancellationToken
        )
        {
            // TODO: Move this Run into the Player System Move Plugin
            await _mediator.Send(
                new RegisterPlayerAction(
                    _idPool.NextId(),
                    PlayerMoveActions.MOVE,
                    new MovePlayerEvent()
                )
            );


            // TODO: Move this Run into the Player System Move Plugin
            await _mediator.Send(
                new RegisterPlayerAction(
                    _idPool.NextId(),
                    PlayerMoveActions.STOP,
                    new StopPlayerEvent()
                )
            );
        }
    }
}
