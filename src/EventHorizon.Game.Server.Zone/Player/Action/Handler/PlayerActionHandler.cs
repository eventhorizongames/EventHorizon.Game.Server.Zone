using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Find;
using EventHorizon.Game.Server.Zone.Player.Actions;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.Actions.Testing.MoveEntity;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Action.Handler
{
    public class PlayerActionHandler : INotificationHandler<PlayerActionEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _playerRepository;

        public PlayerActionHandler(IMediator mediator, IPlayerRepository playerRepository)
        {
            _mediator = mediator;
            _playerRepository = playerRepository;
        }
        public async Task Handle(PlayerActionEvent notification, CancellationToken cancellationToken)
        {
            var player = await _playerRepository.FindById(notification.PlayerId);
            switch (notification.Action)
            {
                case PlayerActions.MOVE:
                    await _mediator.Send(new MovePlayerEvent()
                    {
                        Player = player,
                        MoveDirection = notification.Data
                    });
                    break;
                // TODO: Test Action, Remove in future.
                case PlayerActions.TESTING_PATH_ENTITY_TO_PLAYER:
                    await _mediator.Publish(new MoveEntityToPositionEvent
                    {
                        Position = player.Position.CurrentPosition,
                        EntityId = notification.Data
                    });
                    break;
                // TODO: Test Action, Remove in future.
                case PlayerActions.DECREASE_ENTITY_HP:
                    await _mediator.Publish(new DecreaseHealthPointsEvent
                    {
                        EntityId = notification.Data.entityId,
                        Points = notification.Data.points,
                    });
                    break;
                // TODO: Test Action, Remove in future.
                case PlayerActions.INCREASE_ENTITY_HP:
                    await _mediator.Publish(new IncreaseHealthPointsEvent
                    {
                        EntityId = notification.Data.entityId,
                        Points = notification.Data.points,
                    });
                    break;
            }
        }
    }
}