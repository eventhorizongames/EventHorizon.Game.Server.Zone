using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Player;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using EventHorizon.Zone.System.Player.Events.Info;
using EventHorizon.Zone.System.Player.Events.Zone;
using EventHorizon.Zone.System.Player.ExternalHub;

namespace EventHorizon.Zone.System.Player.Zone
{
    public struct SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        readonly IMediator _mediator;
        readonly IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(
            IMediator mediator,
            IHubContext<PlayerHub> hubContext
        )
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        public async Task<PlayerEntity> Handle(
            SendZoneInfoToPlayerEvent request, 
            CancellationToken cancellationToken
        )
        {
            await _hubContext.Clients
                .Client(
                    request.Player.ConnectionId
                ).SendAsync(
                    "ZoneInfo",
                    await _mediator.Send(
                        new QueryForPlayerZoneInfo(
                            request.Player
                        )
                    )
                );
            return request.Player;
        }
    }
}