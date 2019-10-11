using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.ExternalHub;
using EventHorizon.Zone.System.Player.Plugin.Action;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Action.Handler
{
    public class PlayerClientActionHandler : INotificationHandler<PlayerClientActionEvent>
    {
        readonly IPlayerRepository _playerRepository;
        readonly IHubContext<PlayerHub> _hubContext;

        public PlayerClientActionHandler(
            IPlayerRepository playerRepository,
            IHubContext<PlayerHub> hubContext
        )
        {
            _playerRepository = playerRepository;
            _hubContext = hubContext;
        }
        
        public async Task Handle(
            PlayerClientActionEvent notification,
            CancellationToken cancellationToken
        )
        {
            var player = await _playerRepository.FindById(
                notification.PlayerId
            );

            await _hubContext.Clients
                .Client(
                    player.ConnectionId
                ).SendAsync(
                    "ClientAction", 
                    notification.Action, 
                    notification.Data
                );
        }
    }
}