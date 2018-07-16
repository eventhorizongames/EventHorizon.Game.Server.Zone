using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Model;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Zone.Handler
{
    public class SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        readonly ZoneSettings _zoneSettings;
        readonly IServerState _serverState;
        readonly IEntityRepository _entityRepository;
        readonly IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(IZoneSettingsFactory zoneSettingsFactory, IServerState serverState, IEntityRepository entityRepository, IHubContext<PlayerHub> hubContext)
        {
            _zoneSettings = zoneSettingsFactory.Settings;
            _serverState = serverState;
            _entityRepository = entityRepository;
            _hubContext = hubContext;
        }
        public async Task<PlayerEntity> Handle(SendZoneInfoToPlayerEvent request, CancellationToken cancellationToken)
        {
            var zoneInfo = new
            {
                Player = request.Player,
                MapMesh = _zoneSettings.Map.Mesh,
                Map = await _serverState.Map(),
                EntityList = await _entityRepository.All()
            };
            await _hubContext.Clients.Client(request.Player.ConnectionId).SendAsync("ZoneInfo", zoneInfo);

            return request.Player;
        }
    }
}