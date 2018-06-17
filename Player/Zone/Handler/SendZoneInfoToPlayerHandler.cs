using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Zone.Handler
{
    public class SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        readonly IServerState _serverState;
        readonly IEntityRepository _entityRepository;
        readonly IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(IServerState serverState, IEntityRepository entityRepository, IHubContext<PlayerHub> hubContext)
        {
            _serverState = serverState;
            _entityRepository = entityRepository;
            _hubContext = hubContext;
        }
        public async Task<PlayerEntity> Handle(SendZoneInfoToPlayerEvent request, CancellationToken cancellationToken)
        {
            var zoneInfo = new
            {
                Player = request.Player,
                MapMesh = new // TODO: Read this configuration from DB or something simliar, make dynamic
                {
                    HeightMapUrl = "/Game/Level/Home/Assets/HomeLevel.png",
                    Width = 128,
                    Height = 128,
                    Subdivisions = 200,
                    MinHeight = 0,
                    MaxHeight = 15,
                    Updatable = true,
                },
                Map = await _serverState.Map(),
                EntityList = await _entityRepository.All()
            };
            await _hubContext.Clients.Client(request.Player.ConnectionId).SendAsync("ZoneInfo", zoneInfo);

            return request.Player;
        }
    }
}