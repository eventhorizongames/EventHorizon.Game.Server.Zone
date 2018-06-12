using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Client.Handler
{
    public class ClientActionHandler : INotificationHandler<ClientActionEvent>
    {
        readonly IPlayerRepository _playerRepository;
        readonly IHubContext<PlayerHub> _hubContext;
        public ClientActionHandler(IPlayerRepository playerRepository, IHubContext<PlayerHub> hubContext)
        {
            _playerRepository = playerRepository;
            _hubContext = hubContext;
        }

        public async Task Handle(ClientActionEvent notification, CancellationToken cancellationToken)
        {
            var player = await _playerRepository.FindById(notification.PlayerId);

            await _hubContext.Clients.Client(player.ConnectionId)
                .SendAsync("ClientAction", notification.Action, notification.Data);
        }
    }
}