using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using EventHorizon.Game.Server.Zone.External.Entity;

namespace EventHorizon.Game.Server.Zone.Player.Zone.Handler
{
    public class SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        readonly ZoneMap _zoneMap;
        readonly IServerState _serverState;
        readonly IEntityRepository _entityRepository;
        readonly IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(IZoneMapFactory zoneMapFactory, IServerState serverState, IEntityRepository entityRepository, IHubContext<PlayerHub> hubContext)
        {
            _zoneMap = zoneMapFactory.Map;
            _serverState = serverState;
            _entityRepository = entityRepository;
            _hubContext = hubContext;
        }
        public async Task<PlayerEntity> Handle(SendZoneInfoToPlayerEvent request, CancellationToken cancellationToken)
        {
            var zoneInfo = new
            {
                Player = request.Player,
                MapMesh = _zoneMap.Mesh,
                Map = await _serverState.Map(),
                EntityList = await _entityRepository.All()
            };
            await _hubContext.Clients.Client(request.Player.ConnectionId).SendAsync("ZoneInfo", zoneInfo);

            return request.Player;
        }
    }
}