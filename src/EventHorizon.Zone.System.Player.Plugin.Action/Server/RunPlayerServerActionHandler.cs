using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Plugin.Action.Events;
using EventHorizon.Zone.System.Player.Plugin.Action.State;

namespace EventHorizon.Zone.System.Player.Plugin.Action.Server
{
    public class RunPlayerServerActionHandler : INotificationHandler<RunPlayerServerAction>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _playerRepository;
        readonly PlayerActionRepository _playerActionRepository;

        public RunPlayerServerActionHandler(
            IMediator mediator,
            IPlayerRepository playerRepository,
            PlayerActionRepository playerActionRepository
        )
        {
            _mediator = mediator;
            _playerRepository = playerRepository;
            _playerActionRepository = playerActionRepository;
        }

        public async Task Handle(
            RunPlayerServerAction notification,
            CancellationToken cancellationToken
        )
        {
            var player = await _playerRepository.FindById(
                notification.PlayerId
            );
            if (!player.IsFound())
            {
                return;
            }
            var actionList = _playerActionRepository.Where(
                notification.Action
            );

            foreach (var action in actionList)
            {
                await _mediator.Publish(
                    action.ActionEvent
                        .SetPlayer(
                            player
                        ).SetData(
                            notification.Data
                        )
                );
            }
        }
    }
}