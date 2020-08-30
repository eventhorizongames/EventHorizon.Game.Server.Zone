namespace EventHorizon.Zone.System.Player.ExternalHub
{
    using global::System;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Player.Events.Connected;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public partial class PlayerHub : Hub
    {
        readonly IMediator _mediator;

        public PlayerHub(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            var playerId = GetPlayerId();
            if (playerId == null)
            {
                this.Context.Abort();
                return;
            }
            await _mediator.Publish(
                new PlayerConnectedEvent(
                    playerId,
                    Context.ConnectionId
                )
            );
        }

        public override async Task OnDisconnectedAsync(
            Exception exception
        )
        {
            var playerId = GetPlayerId();
            if (playerId == null)
            {
                return;
            }
            await _mediator.Publish(
                new PlayerDisconnectedEvent(
                    playerId
                )
            );
        }

        private string GetPlayerId()
        {
            return Context.User.Claims
                .FirstOrDefault(
                    claim => claim.Type == "sub"
                )?.Value;
        }
    }
}