using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Actions;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.State;
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
            }
        }
    }
}