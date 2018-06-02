using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Zone.Handler
{
    public class SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        IServerState _serverState;
        IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(IServerState serverState, IHubContext<PlayerHub> hubContext)
        {
            _serverState = serverState;
            _hubContext = hubContext;
        }
        public async Task<PlayerEntity> Handle(SendZoneInfoToPlayerEvent request, CancellationToken cancellationToken)
        {
            var zoneInfo = new {
                Map = await _serverState.Map(),
            };
            await _hubContext.Clients.Client(request.Player.ConnectionId).SendAsync("ZoneInfo", zoneInfo);

            return request.Player;
        }
    }
}