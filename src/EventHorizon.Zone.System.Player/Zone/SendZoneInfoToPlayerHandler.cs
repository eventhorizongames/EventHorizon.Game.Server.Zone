namespace EventHorizon.Zone.System.Player.Zone
{
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Events.Info;
    using EventHorizon.Zone.System.Player.Events.Zone;
    using EventHorizon.Zone.System.Player.ExternalHub;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class SendZoneInfoToPlayerHandler 
        : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<PlayerHub> _hubContext;

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